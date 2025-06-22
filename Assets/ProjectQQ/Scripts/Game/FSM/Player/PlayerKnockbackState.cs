using UnityEngine;

namespace QQ.FSM
{
    public class PlayerKnockbackState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        public PlayerKnockbackState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            actor.PlayerMovement.LockMovement();
            LogHelper.Log("Enter PlayerKnockbackState : �ƾ�");
            actor.SetCanAttack(false);
        }

        public void Update()
        {
            // ����: �Է� ���� ���� ���� ����
            if (actor.PlayerMovement.MoveDirection == Vector2.zero)
                context.ChangeState(context.PlayerIdleState);
            else
                context.ChangeState(context.PlayerMoveState);
        }

        public void Exit()
        {
            actor.PlayerMovement.UnlockMovemnet();
            actor.SetCanAttack(true);
        }

    }
}