using UnityEngine;

namespace QQ.FSM
{
    public class MonsterIdleState : IState
    {
        private readonly Monster monster;
        private readonly MonsterStateContext monsterStateContext;

        public MonsterIdleState(Monster monster, MonsterStateContext monsterStateContext)
        {
            this.monster = monster;
            this.monsterStateContext = monsterStateContext;
        }
        
        public void Enter()
        {
            monster.SetCurAnimation(AnimState.Idle);
        }

        public void Update()
        {
            if (monster.TargetTransform != null && !monster.TargetTransform.GetComponent<Actor>().IsDead)
            {
                monsterStateContext.ChangeState(monsterStateContext.MonsterChaseState);
            }
        }

        public void Exit()
        {
            
        }
    }
}