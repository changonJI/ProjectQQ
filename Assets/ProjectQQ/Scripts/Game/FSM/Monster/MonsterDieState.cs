namespace QQ.FSM
{
    public class MonsterDieState : IState
    {
        private readonly Monster monster;

        public bool IsInputBlocked { get; }

        public MonsterDieState(Monster monster, MonsterStateContext monsterStateContext)
        {
            this.monster = monster;
        }
        
        public void Enter()
        {
            monster.SetCurAnimation(AnimState.Die);
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}