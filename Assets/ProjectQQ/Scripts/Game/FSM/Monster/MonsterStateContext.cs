namespace QQ.FSM
{
    public class MonsterStateContext : BaseStateContext
    {
        public IState MonsterIdleState { get; private set; }
        public IState MonsterChaseState { get; private set; }
        public IState MonsterDieState { get; private set; }

        public MonsterStateContext(Monster monster)
        {
            MonsterIdleState = new MonsterIdleState(monster, this);
            MonsterChaseState = new MonsterChaseState(monster, this);
            MonsterDieState = new MonsterDieState(monster, this);
        }
    }
}