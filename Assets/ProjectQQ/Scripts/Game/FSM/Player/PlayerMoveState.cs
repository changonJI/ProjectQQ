using UnityEngine;

namespace QQ.FSM
{
    public class PlayerMoveState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;
        
        public bool IsInputBlocked => false;

        public PlayerMoveState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            actor.SetCurAnimation(AnimState.Run);
        }

        public void Update()
        {
            if (actor.PlayerMovement.MoveDirection == Vector2.zero)
                actor.StateContext.ChangeState(context.PlayerIdleState);
        }

        public void Exit()
        {
        }

    }
}