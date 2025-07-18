using QQ.FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace QQ
{
    public class Actor : SpineGameObject, IDamageable
    {
        public override GameObjectType Type => GameObjectType.Actor;
        
        public PlayerMovement PlayerMovement { get; private set; }
        private PlayerStatData playerStatData;

        private List<ItemData> inventory;

        // 범위
        private float minDist = float.MaxValue;
        private readonly float attackRadius = 30f;
        private readonly float attackRadiusRange = 900f;
        private readonly Collider2D[] hitEnemy = new Collider2D[100];
        private ContactFilter2D enemyFilter;
        private Transform target = null;

        // 이속
        public override float GetSpeed() => playerStatData.baseSpeed + addSpeed;
        private float addSpeed = 0f;
        // 체력
        private int maxHp() => playerStatData.heartMax;
        private int currentHp;
        public bool IsDead = false;
        
        //마지막 피격 방향
        public Vector2 LastHitDirection { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            InitPlayer();
            InitController();
        }

        protected override void OnStart()
        {
            SetTable();
            stateContext.ChangeState(stateContext.GetIdleState());
            inventory.Add(ItemDataManager.Instance.Get(3));
        }

        protected override void OnFixedUpdate()
        {
            ScanObject();
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
            if(other.gameObject.layer == (int)Layer.Enemy)
            {

            }
            else if(other.gameObject.layer == (int)Layer.Item)
            {

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

        private void ScanObject()
        {
            int count = Physics2D.OverlapCircle(transform.localPosition + new Vector3(0, 9, 0), attackRadius, enemyFilter, hitEnemy);

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

                    LogHelper.Log($"dist : {dist}");

                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = enemy.transform;
                    }
                }
            }

            if (target == null) return;
            if(target.gameObject.activeSelf == false) return;

            Attack(target, minDist);
        }

        #region 공격 + 피격 (수정 예정)

        private void Attack(Transform target, float dist)
        {
            // 현재 FSM 체크
            if (stateContext.GetCurFSMType() == FSMState.Die || stateContext.GetCurFSMType() == FSMState.Knockback) return;

            // 현재 들고 있는 
            foreach(var weapon in inventory)
            {
                //TODO : TableID로 공격 범위 체크
                // 사거리 체크
                if (dist > attackRadiusRange) continue;

                //TODO: Table ID로 id, duration 필요
                // CoolTime 체크
                if (!CoolTimeManager.Instance.IsItemReady(weapon.skillId, 5f)) continue;

                EffectManager.Instance.PlayEffect(weapon.skillId).Forget();
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
#endif

    }
}
