using System;
using QQ.FSM;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QQ
{
    public class Actor : BaseGameObject
    {
        public override GameObjectType Type => GameObjectType.Actor;

        private PlayerStatData playerStatData;
        public PlayerMovement PlayerMovement { get; private set; }
        
        public PlayerStateContext StateContext { get; private set; }
        
        // �÷��̾� ������ �ӽ�
        [SerializeField] private int maxHp = 10;
        private int currentHp;
        public Vector2 LastHitDirection { get; private set; }
        public bool IsDead = false;
        
        // ���� �ӽ�
        private float attackInterval = 1.0f;
        private float attackTimer;
        private bool canAttack = true; // ���� ���� ����
        
        // public override float Speed { get => playerStatData.baseSpeed; }
        public override float Speed { get => 1f; } // �׽�Ʈ�� �ӽ� �ڵ�

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
            RigidBody = GetComponent<Rigidbody2D>();
            if (null == RigidBody)
            {
                LogHelper.LogError($"MovementBase {gameObject.name} ������ٵ�2D�� ����");
            }
            
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

        #region ���� + �ǰ� (���� ����)

        public void PerformAttack()
        {
            Debug.Log("����");
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
            
            Debug.Log($"{gameObject.name} ����: {damage} �� ���� ü��: {currentHp}");

            if (currentHp <= 0)
            {
                OnDie();
                return;
            }

            // FSM ���� ����
            LastHitDirection = (transform.position - transformPosition).normalized;
            ChangeKnockBackState();
        }

        private void OnDie()
        {
            if (IsDead) return;

            IsDead = true;
            ChangeDieState();

            // ���� ���� ó��: ����Ʈ, ����, UI, �ı�
            Debug.Log($"{name} ��� ó�� �Ϸ�");
        }

        #endregion
    }
}
