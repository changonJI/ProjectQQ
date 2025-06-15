using UnityEngine;

namespace QQ.FSM
{
    public class PlayerIdleState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public PlayerIdleState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            actor.SetCurAnimation(AnimState.Idle);
        }

        public void Update()
        {
            if (actor.MoveDirection != Vector2.zero)
                actor.StateContext.ChangeState(context.PlayerMoveState);
        }

        public void Exit()
        {
        }
    }
}