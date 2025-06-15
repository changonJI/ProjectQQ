using UnityEngine;

namespace QQ.FSM
{
    public class PlayerRollState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;
        
        // 임시
        private float rollDuration = 0.5f; // 롤 시간
        private float elapsedTime;
        private float rollSpeed = 12f;
        private Vector2 rollDirection;

        public PlayerRollState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            // todo : 구르기 임시 계산
            elapsedTime = 0f;
            rollDirection = actor.MoveDirection.normalized;
            actor.ForceMove(rollDirection * rollSpeed);
            
            actor.SetCurAnimation(AnimState.Roll, 1.0f); // roll 애니메이션 재생
            actor.SetCanAttack(false);
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= rollDuration)
            {
                // 롤 종료 후 다시 상태 전환
                if (actor.MoveDirection == Vector2.zero)
                    context.ChangeState(context.PlayerIdleState);
                else
                    context.ChangeState(context.PlayerMoveState);
            }
        }

        public void Exit()
        {
            actor.StopForceMove();
            actor.SetCanAttack(true);
        }
    }
}