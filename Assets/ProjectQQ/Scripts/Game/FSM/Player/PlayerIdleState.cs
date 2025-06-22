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
            actor.PlayerMovement.UnlockMovemnet();
            actor.SetCurAnimation(AnimState.Idle);
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}