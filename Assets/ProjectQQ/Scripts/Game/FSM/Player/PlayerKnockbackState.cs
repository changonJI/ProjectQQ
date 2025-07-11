using UnityEngine;

namespace QQ.FSM
{
    public class PlayerKnockbackState : IState
    {
        public FSMState GetFSMType() => FSMState.Knockback;
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public PlayerKnockbackState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            actor.PlayerMovement.SetMoveBlock(true);
            LogHelper.Log("Enter PlayerKnockbackState : 아얏");
            actor.SetCanRangedAttack(false);
        }

        public void Update()
        {
            // 복귀: 입력 방향 보고 상태 결정
            if (actor.PlayerMovement.InputDirection == Vector2.zero)
                context.ChangeState(context.GetIdleState());
            else
                context.ChangeState(context.GetMoveState());
        }

        public void Exit()
        {
            actor.PlayerMovement.SetMoveBlock(false);
            actor.SetCanRangedAttack(true);
        }

    }
}