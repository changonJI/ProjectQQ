using UnityEngine;

namespace QQ.FSM
{
    public class MonsterChaseState : IState
    {
        private readonly Monster monster;
        private readonly MonsterStateContext context;

        public bool IsInputBlocked { get; }

        public MonsterChaseState(Monster monster, MonsterStateContext monsterStateContext)
        {
            this.monster = monster;
            this.context = monsterStateContext;
        }
        
        public void Enter()
        {
            monster.SetCurAnimation(AnimState.Run);
        }

        public void Update()
        {
            if (monster.TargetTransform == null  || monster.TargetTransform.GetComponent<Actor>().IsDead)
            {
                context.ChangeState(context.MonsterIdleState);
                return;
            }

            Vector2 dir = (monster.TargetTransform.position - monster.transform.position).normalized;
            monster.MonsterMovement.SetDirection(dir);
        }

        public void Exit()
        {
            monster.MonsterMovement.SetDirection(Vector2.zero);
        }
    }
}