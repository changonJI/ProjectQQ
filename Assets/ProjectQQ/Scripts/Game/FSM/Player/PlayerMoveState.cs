namespace QQ.FSM
{
    public class PlayerMoveState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public PlayerMoveState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            actor.PlayerMovement.SetMoveLock(false);
            actor.SetCurAnimation(AnimState.Run);
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }

    }
}