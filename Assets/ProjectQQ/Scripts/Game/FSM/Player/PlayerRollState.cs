using UnityEngine;

namespace QQ.FSM
{
    public class PlayerRollState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;
        
        // �ӽ�
        private float rollDuration = 0.5f; // �� �ð�
        private float elapsedTime;
        private float rollSpeed = 12f;
        private Vector2 rollDirection;

        public PlayerRollState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            // todo : ������ �ӽ� ���
            elapsedTime = 0f;
            rollDirection = actor.MoveDirection.normalized;
            actor.ForceMove(rollDirection * rollSpeed);
            
            actor.SetCurAnimation(AnimState.Roll, 1.0f); // roll �ִϸ��̼� ���
            actor.SetCanAttack(false);
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= rollDuration)
            {
                // �� ���� �� �ٽ� ���� ��ȯ
                if (actor.MoveDirection == Vector2.zero)
                    context.ChangeState(context.PlayerIdleState);
                else
                    context.ChangeState(context.PlayerMoveState);
            }
        }

        public void Exit()
        {
            actor.StopForceMove();
            actor.SetCanAttack(true);
        }
    }
}