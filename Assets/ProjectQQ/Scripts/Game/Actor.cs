using System;
using QQ.FSM;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QQ
{
    public class Actor : BaseGameObject
    {
        public override GameObjectType Type => GameObjectType.Actor;

        private PlayerStatData playerStatData;
        private PlayerMovement playerMovement;
        
        public PlayerStateContext StateContext { get; private set; }
        
        // �÷��̾� ������ �ӽ�
        public Vector2 MoveDirection { get; private set; }
        [SerializeField] private int maxHp = 10;
        private int currentHp;
        public Vector2 LastHitDirection { get; private set; }
        public bool IsDead = false;
        
        // ���� �ӽ�
        private float attackInterval = 1.0f;
        private float attackTimer;
        private bool canAttack = true; // ���� ���� ����

        public override void Init()
        {
            playerStatData = new PlayerStatData();
            
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
            StateContext = new PlayerStateContext(this);
            playerMovement = GetComponent<PlayerMovement>();
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveMoveInputEvent(OnMoveInput);
            InputManager.Instance.RemoveRollInputEvent(OnRollInput);
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
            InputManager.Instance.AddMoveInputEvent(OnMoveInput);
            InputManager.Instance.AddRollInputEvent(OnRollInput);
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

        private void OnMoveInput(Vector2 dir)
        {
            MoveDirection = dir;
        }

        private void OnRollInput()
        {
            if (StateContext == null) return;
            
            StateContext.ChangeState(StateContext.PlayerRollState);
        }

        public void ForceMove(Vector2 velocity)
        {
            // playerMovement.han(velocity);
        }

        public void StopForceMove()
        {
            // playerMovement.ClearOverrideVelocity();
        }

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
            StateContext.ChangeState(StateContext.PlayerKnockbackState);
        }

        private void OnDie()
        {
            if (IsDead) return;

            IsDead = true;
            StateContext.ChangeState(StateContext.PlayerDieState);

            // ���� ���� ó��: ����Ʈ, ����, UI, �ı�
            Debug.Log($"{name} ��� ó�� �Ϸ�");
        }
    }
}
