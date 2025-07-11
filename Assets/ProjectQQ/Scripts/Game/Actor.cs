using ProjectQQ.Scripts.Game.Actions.Player;
using QQ.FSM;
using UnityEngine;

namespace QQ
{
    public class Actor : SpineGameObject, IDamageable
    {
        public override GameObjectType Type => GameObjectType.Actor;
        
        [SerializeField] private PlayerMeleeAttack meleeAttack;
        [SerializeField] private PlayerRangedAttack rangedAttack;
        [SerializeField] private PlayerItemCollector itemCollector;

        public PlayerMovement PlayerMovement { get; private set; }
        private PlayerStatData playerStatData;
        // 이속
        public override float GetSpeed() => playerStatData.baseSpeed + addSpeed;
        private float addSpeed = 0f;
        // 체력
        private int maxHp() => playerStatData.heartMax;
        private int currentHp;
        public bool IsDead = false;
        // 공격
        private bool canMeleeAttack = false;
        private bool canRangedAttack = false;
        private bool canCollectItem = false;

        public Vector2 LastHitDirection { get; private set; }

        public override void Init()
        {
            IsDead = false;

            playerStatData = new PlayerStatData();
            stateContext = new PlayerStateContext(this);
        }

        public override void SetData(int id)
        {
            var data = PlayerStatDataManager.Instance.Get(id);

            playerStatData.Set(data);
        }

        protected override void OnAwake()
        {
            base.OnAwake();
 
            PlayerMovement = gameObject.AddComponent<PlayerMovement>(this);
            
            InputManager.Instance.AddRollInputEvent(ChangeRollState);

            meleeAttack.OnMeleeEntered += SetCanMeleeAttack;
            rangedAttack.OnRangedAttack += SetCanRangedAttack;
            itemCollector.OnItemCollected += SetCanCollectItem;
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveRollInputEvent(ChangeRollState);

            meleeAttack.OnMeleeEntered -= SetCanMeleeAttack;
            rangedAttack.OnRangedAttack -= SetCanRangedAttack;
            itemCollector.OnItemCollected -= SetCanCollectItem;
        }

        protected override void OnDisabled()
        {
        }

        protected override void OnEnabled()
        {
        }

        protected override void OnFixedUpdate()
        {
            if (canCollectItem)
                itemCollector.TryCollectItems();
            if(canMeleeAttack)
                meleeAttack.Attack();
            if(canRangedAttack)
                rangedAttack.Attack();
        }

        protected override void OnLateUpdate()
        {
        }

        protected override void OnStart()
        {
            stateContext.ChangeState(stateContext.GetIdleState());
        }

        protected override void OnUpdate()
        {
            if (status.HasStatus(StatusEffectController.StatusEffect.Stunned)) return;

            stateContext.Update();
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

        #region 공격 + 피격 (수정 예정)

        public void PerformAttack()
        {
            //Debug.Log("공격");
        }

        public void SetCanRangedAttack(bool value) => canRangedAttack = value;
        public void SetCanMeleeAttack(bool value) => canMeleeAttack = value;
        public void SetCanCollectItem(bool value) => canCollectItem = value;
        

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

        #endregion

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

        public void TakeDamage(int damage, Vector3 transformPosition)
        {
            if(IsDead) return;
            
            if(status != null && status.HasStatus(StatusEffectController.StatusEffect.Invincible)) return;
            
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
    }
#endif
}
