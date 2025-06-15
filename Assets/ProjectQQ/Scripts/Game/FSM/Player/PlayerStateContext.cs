namespace QQ.FSM
{
    public class PlayerStateContext
    {
        private IState currentState;
        
        public IState PlayerIdleState { get; private set; }
        public IState PlayerMoveState { get; private set; }
        public IState PlayerRollState { get; private set; }
        public IState PlayerKnockbackState { get; private set; }
        public IState PlayerDieState { get; private set; }

        public PlayerStateContext(Actor actor)
        {
            PlayerIdleState = new PlayerIdleState(actor, this);
            PlayerMoveState = new PlayerMoveState(actor, this);
            PlayerRollState = new PlayerRollState(actor, this);
            PlayerKnockbackState = new PlayerKnockbackState(actor, this);
            PlayerDieState = new PlayerDieState(actor); 
        }

        public void ChangeState(IState newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}