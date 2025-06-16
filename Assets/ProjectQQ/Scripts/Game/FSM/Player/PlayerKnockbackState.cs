using UnityEngine;

namespace QQ.FSM
{
    public class PlayerKnockbackState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public bool IsInputBlocked => true;

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

            LogHelper.Log("Enter PlayerKnockbackState : 아얏");
            actor.SetCanAttack(false);
            // actor.SetCurAnimation(AnimState.Hit);
        }

        public void Update()
        {
            // 복귀: 입력 방향 보고 상태 결정
            if (actor.PlayerMovement.MoveDirection == Vector2.zero)
                context.ChangeState(context.PlayerIdleState);
            else
                context.ChangeState(context.PlayerMoveState);
        }

        public void Exit()
        {
            actor.SetCanAttack(true);
        }

    }
}