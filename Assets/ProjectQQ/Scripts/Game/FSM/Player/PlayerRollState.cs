using UnityEngine;

namespace QQ.FSM
{
    public class PlayerRollState : IState
    {
        private readonly Actor actor;
        private readonly PlayerStateContext context;
        
        public bool IsInputBlocked => true;
        
        // 임시
        private float rollDuration = 0.5f; // 롤 시간
        private float elapsedTime;
        private float rollSpeed = 1f;
        private Vector2 rollDirection;
        private bool isRolling = false;

        public PlayerRollState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            // todo : 구르기 임시 계산
            
            isRolling = true;
            
            elapsedTime = 0f;
            rollDirection = actor.PlayerMovement.MoveDirection.normalized;
            
            actor.SetCurAnimation(AnimState.Roll, 1.0f); // roll 애니메이션 재생
            actor.SetCanAttack(false);
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= rollDuration)
            {
                // 롤 종료 후 다시 상태 전환
                if (actor.PlayerMovement.MoveDirection == Vector2.zero)
                    context.ChangeState(context.PlayerIdleState);
                else
                    context.ChangeState(context.PlayerMoveState);
            }
        }

        public void Exit()
        {
            actor.SetCanAttack(true);
        }

    }
}