namespace QQ.FSM
{
    public class MonsterStateContext
    {
        private IState currentState;
        
        public IState MonsterIdleState { get; private set; }
        public IState MonsterChaseState { get; private set; }
        public IState MonsterDieState { get; private set; }

        public MonsterStateContext(Monster monster)
        {
            MonsterIdleState = new MonsterIdleState(monster, this);
            MonsterChaseState = new MonsterChaseState(monster, this);
            MonsterDieState = new MonsterDieState(monster, this);
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