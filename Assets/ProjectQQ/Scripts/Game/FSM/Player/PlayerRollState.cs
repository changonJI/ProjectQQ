using UnityEngine;

namespace QQ.FSM
{
    public class PlayerRollState : IState
    {
        public FSMState GetFSMType() => FSMState.Roll;
        private readonly Actor actor;
        private readonly PlayerStateContext context;

        private bool isFinished => processTime >= duration;

        // 구르기 모션 시간
        private float duration = 0f;
        // 진행시간
        private float processTime;
        // 더해지는 속도 값
        private float addSpeed = 0f;

        // 구르기 힘(eased * rollPower)
        private float rollPower = 5f;
        // 구르기 anim의 속도 값
        private float easeSpeed = 5f;

        public PlayerRollState(Actor actor, PlayerStateContext playerStateContext)
        {
            this.actor = actor;
            this.context = playerStateContext;
        }
        
        public void Enter()
        {
            Init();
            actor.SetCurAnimation(AnimState.Roll); // roll 애니메이션 재생
        }

        private void Init()
        {
            actor.PlayerMovement.SetDirectionLock(true);
            actor.PlayerMovement.SetMoveDirectionToLast();
            duration = actor.GetAnimDuration(AnimState.Roll);

            processTime = 0f;
            addSpeed = 0f;
        }

        public void Update()
        {
            if (isFinished)
            {
                if (actor.PlayerMovement.InputDirection == Vector2.zero)
                    context.ChangeState(context.GetIdleState());
                else
                    context.ChangeState(context.GetMoveState());
            }
            else
                EaseOut();
        }

        public void Exit()
        {
            actor.CalcAddSpeed(-addSpeed);
            actor.PlayerMovement.SetDirectionLock(false);
        }

        /// <summary>
        /// 처음에 빠르게 뒤로 갈수록 느려짐_지수함수를 이용한 ease-out
        /// Mathf.Clamp01 사용하여 0 ~ 1 사이 값 보간
        /// 지수함수 : e(자연상수)의 거듭제곱 값을 반환
        /// </summary>
        private void EaseOut()
        {
            processTime += Time.deltaTime / duration;
            // 0(1부터 시작) 1(2.71828...까지) 사이로 제한 => duration 값이 변동될수있을듯하여 clamp01 => clamp 사용
            processTime = Mathf.Clamp(processTime, 0, duration);

            // 역수값(끝 지점에서 정확한 값 리턴하기 위해)
            float normalize = 1f / (1f - Mathf.Exp(-easeSpeed));
            float eased = (1f - Mathf.Exp(-easeSpeed * processTime)) * normalize;

            addSpeed = eased * rollPower;

            LogHelper.Log(addSpeed);
            actor.SetAddSpeed(addSpeed);
        }

        /// <summary>
        /// 처음엔 천천히 뒤로 갈수록 빠르게_로그함수를 이용한 ease-in
        /// Mathf.Log10을 이용하여 0 ~ 1 사이 값 보간
        /// 로그함수 : log10(x)의 값을 반환 (mathf.log10 사용)
        /// </summary>
        private void EaseIn(GameObject obj, float scale = 9f)
        {
            processTime += Time.deltaTime / duration;
            processTime = Mathf.Clamp01(processTime);

            // +1 : Log10(0) = -∞ 이므로 0을 피하기 위해 1을 더함.
            // Log10(1) = 0 => 10^0 = 1
            float maxValue = 1f / Mathf.Log10(scale + 1f);
            float eased = Mathf.Log10(processTime * scale + 1f) * maxValue;
        }
    }
}