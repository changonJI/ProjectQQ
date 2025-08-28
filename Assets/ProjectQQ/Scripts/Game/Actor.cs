using ProjectQQ.Scripts.UI.Popup;
using QQ.FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace QQ
{
    public class Actor : SpineGameObject, IDamageable
    {
        public override GameObjectType ObjType => GameObjectType.Actor;
        
        public PlayerMovement PlayerMovement { get; private set; }
        private PlayerStatData playerStatData;
        
        // 레벨
        private int level = 1;
        private int currentExp = 0;
        
        public int Level => level;
        public int Exp => currentExp;

        // 인벤토리
        private List<ItemData> inventory;

        // 공격 범위 범위
        private float minDist = float.MaxValue;
        private float attackRadius;
        // attackRadius 제곱값(sqrMagnitude 값 계산)
        private float attackRadiusRange => Mathf.Pow(attackRadius, 2);

        private readonly Collider2D[] hitEnemy = new Collider2D[100];
        private ContactFilter2D enemyFilter;
        private Transform target = null;

        // 아이템 pull 범위
        private float itemRadius;
        private readonly Collider2D[] hitItem = new Collider2D[100];
        private ContactFilter2D itemFilter;

        // 이속
        public override float GetSpeed() => playerStatData.baseSpeed + addSpeed;
        private float addSpeed = 0f;
        // 체력
        private int maxHp() => playerStatData.heartMax;
        /// <summary>
        /// 최초 1회(Init) heartMax로 체크
        /// </summary>
        private int currentHp;
        public bool IsDead = false;
        
        //마지막 피격 방향
        public Vector2 LastHitDirection { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            SetLayer(gameObject, GameObjectType.Actor);
            InitPlayer();
            InitController();
        }

        protected override void OnStart()
        {
            SetTable();
            stateContext.ChangeState(stateContext.GetIdleState());
        }

        protected override void OnFixedUpdate()
        {
            ScanMonsterObject();
            ScanItemObject();
        }

        protected override void OnUpdate()
        {
            if (status.HasStatus(StatusEffectController.StatusEffect.Stunned)) return;

            stateContext.Update();
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveRollInputEvent(ChangeRollState);
            ListPool<ItemData>.Release(inventory);
        }

        protected override void OnTriggerEnter2Ded(Collider2D other)
        {
            if(other.gameObject.layer == (int)Layer.Item)
            {
                if (other.gameObject.TryGetComponent<ICollectable>(out var item))
                {
                    item.Collect(this);
                }
            }
        }

        #region FSM
        private void ChangeMoveState()
        {
            if(PlayerMovement.IsMoving())
                stateContext.ChangeState(stateContext.GetMoveState());
            else
                stateContext.ChangeState(stateContext.GetIdleState());
        }

        private void ChangeRollState()
        {
            stateContext.ChangeState(stateContext.GetRollState());
        }

        private void ChangeKnockBackState()
        {
            stateContext.ChangeState(stateContext.GetKnockbackState());
        }

        private void ChangeDieState()
        {
            stateContext.ChangeState(stateContext.GetDieState());
        }
        #endregion

        #region State Detail
        public float GetAddSpeed() => addSpeed;
        public void SetAddSpeed(float speed) => addSpeed = speed;
        public void CalcAddSpeed(float speed) => addSpeed += speed;
        
        #endregion

        private void ScanMonsterObject()
        {
            int count = Physics2D.OverlapCircle(transform.localPosition, attackRadius, enemyFilter, hitEnemy);

            minDist = float.MaxValue;

            if (count == 0)
            {
                target = null;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var enemy = hitEnemy[i];

                    if (enemy == null) continue;

                    float dist = Vector2.SqrMagnitude(enemy.transform.localPosition - transform.localPosition);

                    // LogHelper.Log($"dist : {dist}");

                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = enemy.transform;
                    }
                }
            }

            if (target == null) return;
            if(target.gameObject.activeSelf == false) return;

            Attack(minDist);
        }

        private void ScanItemObject()
        {
            int count = Physics2D.OverlapCircle(transform.localPosition, itemRadius, itemFilter, hitItem);

            for (int i = 0; i < count; i++)
            {
                var item = hitItem[i];

                if (item == null) continue;
                if(item.gameObject.activeSelf == false) continue;

                Vector3 dir = (transform.localPosition - item.transform.localPosition).normalized;

                // 자석효과
                item.transform.localPosition += (dir * GameConf.ItemGetSpeed * Time.fixedDeltaTime);
            }
        }

        #region 공격 + 피격 (수정 예정)

        private void Attack(float dist)
        {
            // 현재 FSM 체크
            if (stateContext.GetCurFSMType() == FSMState.Die || stateContext.GetCurFSMType() == FSMState.Knockback) return;

            // 현재 들고 있는 
            foreach(var weapon in inventory)
            {
                var skillData = SkillDataManager.Instance.Get(weapon.skillId);
                if (skillData.id <= 0) return;

                float weaponRange = skillData.range;

                //NOTE: Bullet형 Range값 체크 필요
                //if (dist > Mathf.Pow(weaponRange, 2)) continue;
                if (dist > attackRadiusRange) continue;

                float cooltime = skillData.cooltime * 0.001f;
                // CoolTime 체크
                if (!CoolTimeManager.Instance.IsItemReady(weapon.skillId, cooltime)) continue;

                SkillManager.Instance.UseSkill(weapon.skillId, transform.localPosition).Forget();
            }
        }

        private void OnDie()
        {
            if (IsDead) return;

            IsDead = true;
            ChangeDieState();

            // 죽음 관련 처리: 이펙트, 사운드, UI, 파괴
            Debug.Log($"{name} 사망 처리 완료");
        }

        /// <summary>
        /// 내적을 활용한 앞 뒤 구분. 양수 : 예각, 0 : 직각, 음수 : 둔각
        /// </summary>
        private bool IsBackAttack()
        {
            var dot = Vector3.Dot(PlayerMovement.MoveDirection, LastHitDirection);
            
            if (dot < 0) return true;

            return false;
        }

        public void TakeDamage(int damage, Vector3 transformPosition)
        {
            if (IsDead) return;

            if (status != null && status.HasStatus(StatusEffectController.StatusEffect.Invincible)) return;

            currentHp -= damage;
            currentHp = Mathf.Max(currentHp, 0);

            Debug.Log($"{gameObject.name} 피해: {damage} → 남은 체력: {currentHp}");

            if (currentHp <= 0)
            {
                OnDie();
                return;
            }

            // FSM 상태 전이
            LastHitDirection = (transform.position - transformPosition).normalized;
            ChangeKnockBackState();
        }

        public void TakeStatus(SkillOptionType type, float value = 0)
        {
            switch (type)
            {
                case SkillOptionType.Xp_Pull:
                    itemRadius += value;
                    break;
                case SkillOptionType.Heal:
                    int addHp = Mathf.Clamp((int)value + currentHp, currentHp, maxHp());
                    currentHp = addHp;
                    break;
                case SkillOptionType.MoveSpdUp:
                case SkillOptionType.MoveSpdDown:
                    CalcAddSpeed(value);
                    break;
                case SkillOptionType.Stun:
                    status.ApplyStatus(StatusEffectController.StatusEffect.Stunned, value);
                    break;
                case SkillOptionType.Invincible:
                    status.ApplyStatus(StatusEffectController.StatusEffect.Invincible, value);
                    break;

                case SkillOptionType.None:
                case SkillOptionType.Damage:
                case SkillOptionType.Explosion:
                default:
                    // 기본적으로 데미지 타입은 처리하지 않음
                    // Explosion은 BulletDamage에서 처리
                    break;
            }
        }
        #endregion

        public Transform GetTarget()
        {
            return target;
        }

        private void InitPlayer()
        {
            IsDead = false;

            playerStatData = new PlayerStatData();
            stateContext = new PlayerStateContext(this);
            inventory = ListPool<ItemData>.Get();

            enemyFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = 1 << (int)Layer.Enemy,
                useTriggers = true
            };

            itemFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = 1 << (int)Layer.Item,
                useTriggers = true
            };

            currentHp = maxHp();
            attackRadius = GameConf.AttackRadius;
            itemRadius = GameConf.ItemRadius;
            
            GameManager.Instance.RegisterActor(this);

            AddItem(ItemDataManager.Instance.Get(3));
        }

        private void InitController()
        {
            PlayerMovement = gameObject.AddComponent<PlayerMovement>(this);
            InputManager.Instance.AddRollInputEvent(ChangeRollState);
        }

        private void SetTable()
        {
            var data = PlayerStatDataManager.Instance.Get(tableID);

            playerStatData.Set(data);
        }


        /// <summary>
        /// 강체끼리 부딪혔을때 값이 증가하여 움직이는 버그를 막기위해 강제 초기화
        /// </summary>
        public void InitVelocity()
        {
            rigid.linearVelocity = Vector2.zero;
        }

        public void SetCollider(bool isActive)
        {
            col.enabled = isActive;
        }

#if UNITY_EDITOR
        [SerializeField] bool onActorState = true;
        private void OnGUI()
        {
            if (onActorState)
            {
                GUIStyle myStyle = new GUIStyle(GUI.skin.label);
                myStyle.fontSize = 50;
                myStyle.normal.textColor = Color.green;
                GUI.Label(new Rect(20, 40, Screen.width * 0.3f, Screen.height * 0.3f), stateContext.GetCurFSMType().ToString(), myStyle);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.localPosition, attackRadius);
        }
#endif

        /// <summary>
        /// 체력 회복
        /// </summary>
        /// <param name="healAmount">아이템 회복량</param>
        public void Heal(int healAmount)
        {
            if (IsDead) return;

            currentHp += healAmount;
            currentHp = Mathf.Min(currentHp, maxHp());
            
            Debug.Log($"{gameObject.name} <UNK>: {currentHp}");
        }

        #region LevelSystem

        /// <summary>
        /// 다음 레벨까지 필요한 경험치
        /// </summary>
        public int ExpToNextLevel
        {
            get
            {
                var data = ExpDataManager.Instance.Get(level);
                return data.NextLvExp;
            }
        }

        /// <summary>
        /// 최대 레벨 여부 확인
        /// </summary>
        public bool IsMaxLevel
        {
            get
            {
                var data = ExpDataManager.Instance.Get(level);
                return data.NextLvExp <= 0;
            }
        }
        
        public void AddExp(int amount)
        {
            if (IsDead || IsMaxLevel) return;

            currentExp += amount;

            while (!IsMaxLevel && currentExp >= ExpToNextLevel)
            {
                currentExp -= ExpToNextLevel;
                level++;

                OnLevelUp();
            }

            Debug.Log($"경험치 획득: +{amount} → 현재: {currentExp}/{ExpToNextLevel} (Lv.{level})");
        }
        
        private void OnLevelUp()
        {
            Debug.Log($"레벨업 → Lv.{level}");
            
            GameManager.Instance.TimeScaleChanger(true); // 게임 일시 정지
            UIClearReward.Instantiate(); // 룰렛 UI 호출

            // 능력치 증가에 따른 플레이어 스탯 변경
            // 이펙트, 사운드, UI 알림
        }

        #endregion

        #region Inventory

        public List<ItemData> GetInventory() => inventory;

        public void AddItem(ItemData data)
        {
            inventory.Add(data);
        }

        public void ReplaceItem(ItemData oldItem, ItemData newItem)
        {
            inventory.Remove(oldItem);
            inventory.Add(newItem);
        }

        #endregion
    }
}
