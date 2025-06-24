using QQ.FSM;
using UnityEngine;

namespace QQ
{
    public class Actor : SpineGameObject
    {
        public override GameObjectType Type => GameObjectType.Actor;

        private PlayerStatData playerStatData;
        public PlayerMovement PlayerMovement { get; private set; }
        
        public PlayerStateContext StateContext { get; private set; }
        
        // 플레이어 데이터 임시
        [SerializeField] private int maxHp = 10;
        private int currentHp;
        public Vector2 LastHitDirection { get; private set; }
        public bool IsDead = false;
        
        // 공격 임시
        private float attackInterval = 1.0f;
        private float attackTimer;
        private bool canAttack = true; // 공격 가능 여부
        
        // public override float Speed { get => playerStatData.baseSpeed; }
        public override float Speed { get => 1f; } // 테스트용 임시 코드

        public override void Init()
        {
            playerStatData = new PlayerStatData();
            StateContext = new PlayerStateContext(this);
            
            currentHp = maxHp;
            IsDead = false;
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
            
            PlayerMovement.OnMove += ChangeMoveState;
            PlayerMovement.OnRoll += ChangeRollState;
        }

        protected override void OnDestroyed()
        {
            PlayerMovement.OnMove -= ChangeMoveState;
            PlayerMovement.OnRoll -= ChangeRollState;
        }

        protected override void OnDisabled()
        {
        }

        protected override void OnEnabled()
        {
        }

        protected override void OnFixedUpdate()
        {
        }

        protected override void OnLateUpdate()
        {
        }

        protected override void OnStart()
        {
            StateContext.ChangeState(StateContext.PlayerIdleState);
        }

        protected override void OnUpdate()
        {
            if (status.HasStatus(StatusEffectController.StatusEffect.Stunned)) return;

            StateContext.Update();
            
            if (canAttack)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackInterval)
                {
                    attackTimer = 0f;
                    PerformAttack();
                }
            }
        }

        #region FSM
        
        private void ChangeMoveState(Vector2 dir)
        {
            if(dir != Vector2.zero)
                StateContext.ChangeState(StateContext.PlayerMoveState);
            else
                StateContext.ChangeState(StateContext.PlayerIdleState);
        }

        private void ChangeRollState()
        {
            StateContext.ChangeState(StateContext.PlayerRollState);
        }

        private void ChangeKnockBackState()
        {
            StateContext.ChangeState(StateContext.PlayerKnockbackState);
        }

        private void ChangeDieState()
        {
            StateContext.ChangeState(StateContext.PlayerDieState);
        }

        #endregion

        #region 공격 + 피격 (수정 예정)

        public void PerformAttack()
        {
            Debug.Log("공격");
        }

        public void SetCanAttack(bool value)
        {
            canAttack = value;
            if (!value)
                attackTimer = 0f;
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

        private void OnDie()
        {
            if (IsDead) return;

            IsDead = true;
            ChangeDieState();

            // 죽음 관련 처리: 이펙트, 사운드, UI, 파괴
            Debug.Log($"{name} 사망 처리 완료");
        }

        #endregion
    }
}
