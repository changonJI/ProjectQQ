using UnityEngine;

namespace QQ.FSM
{
    public class PlayerIdleState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public bool IsInputBlocked => false;

        public PlayerIdleState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            LogHelper.Log("Enter PlayerIdleState");
            actor.SetCurAnimation(AnimState.Idle);
        }

        public void Update()
        {
            if (actor.PlayerMovement.MoveDirection != Vector2.zero)
                actor.StateContext.ChangeState(context.PlayerMoveState);
        }

        public void Exit()
        {
        }
    }
}