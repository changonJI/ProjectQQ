using UnityEngine;

namespace QQ.FSM
{
    public class MonsterChaseState : IState
    {
        public FSMState GetFSMType() => FSMState.Move;
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

            // actor의 위치 쫒기
            monster.MonsterMovement.SetDestination(monster.TargetTransform.position);
        }

        public void Exit()
        {
            monster.MonsterMovement.SetDestination(monster.transform.position);
        }
    }
}