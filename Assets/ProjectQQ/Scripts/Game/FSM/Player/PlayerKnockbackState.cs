using UnityEngine;

namespace QQ.FSM
{
    public class PlayerKnockbackState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public PlayerKnockbackState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            // elapsedTime = 0f;
            // knockbackDir = actor.LastHitDirection.normalized;
            // actor.ForceMove(knockbackDir * knockbackPower);

            LogHelper.Log("�ƾ�");
            actor.SetCanAttack(false);
            // actor.SetCurAnimation(AnimState.Hit);
        }

        public void Update()
        {
            // ����: �Է� ���� ���� ���� ����
            if (actor.MoveDirection == Vector2.zero)
                context.ChangeState(context.PlayerIdleState);
            else
                context.ChangeState(context.PlayerMoveState);
        }

        public void Exit()
        {
            actor.StopForceMove();
            actor.SetCanAttack(true);
        }
    }
}