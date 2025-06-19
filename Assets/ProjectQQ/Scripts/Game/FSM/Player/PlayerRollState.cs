using UnityEngine;

namespace QQ.FSM
{
    public class PlayerRollState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;
        
        public bool IsInputBlocked => true;
        
        // �ӽ�
        private float rollDuration = 0.5f; // �� �ð�
        private float elapsedTime;
        private float rollSpeed = 1f;
        private Vector2 rollDirection;
        private bool isRolling = false;

        public PlayerRollState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            // todo : ������ �ӽ� ���
            
            isRolling = true;
            
            elapsedTime = 0f;
            rollDirection = actor.PlayerMovement.MoveDirection.normalized;
            
            actor.SetCurAnimation(AnimState.Roll, 1.0f); // roll �ִϸ��̼� ���
            actor.SetCanAttack(false);
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= rollDuration)
            {
                // �� ���� �� �ٽ� ���� ��ȯ
                if (actor.PlayerMovement.MoveDirection == Vector2.zero)
                    context.ChangeState(context.PlayerIdleState);
                else
                    context.ChangeState(context.PlayerMoveState);
            }
        }

        public void Exit()
        {
            actor.SetCanAttack(true);
        }

    }
}