using UnityEngine;

namespace QQ.FSM
{
    public class PlayerRollState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public PlayerRollState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            actor.PlayerMovement.LockMovement();
            actor.SetCurAnimation(AnimState.Roll, 1.0f); // roll 애니메이션 재생
        }

        public void Update()
        {
            if (actor.PlayerMovement.isRollStart == false)
            {
                if(actor.PlayerMovement.MoveDirection != Vector2.zero)
                    context.ChangeState(context.PlayerMoveState);
                else
                    context.ChangeState(context.PlayerIdleState);
            }
        }

        public void Exit()
        {
            actor.PlayerMovement.UnlockMovemnet();
        }

    }
}