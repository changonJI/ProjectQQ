using UnityEngine;

namespace QQ
{
    public class RollEff : EffectSystem
    {
        // NOTE: 구르기 거리
        private readonly Vector3 endPosValue = new Vector3(10f, 10f);

        public override void Init()
        {
            base.Init();

            endPos = owner.transform.localPosition + Vector3.Scale(endPosValue, owner.PlayerMovement.MoveDirection);
            owner.PlayerMovement.SetRollState(true);
        }
        protected override void OnAwake()
        {
            base.OnAwake();
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void OnFixedUpdate()
        {
            if(IsFinished)
            {
                owner.PlayerMovement.SetRollState(false); // 구르기 상태 해제
                PoolManager.Instance.ReleaseObject(gameObject);
                return;
            }

            EaseOut();
        }

        /// <summary>
        /// 처음에 빠르게 뒤로 갈수록 느려짐_지수함수를 이용한 ease-out
        /// Mathf.Clamp01 사용하여 0 ~ 1 사이 값 보간
        /// 지수함수 : e(자연상수)의 거듭제곱 값을 반환
        /// </summary>
        private void EaseOut()
        {
            processTime += Time.deltaTime / duration;
            // 0(1부터 시작) 1(2.71828...까지) 사이로 제한 => duration 값이 변동될수있을듯
            processTime = Mathf.Clamp(processTime, 0, duration); 

            float maxValue = 1f / (1f - Mathf.Exp(-speed));
            float eased = (1f - Mathf.Exp(-speed * processTime)) * maxValue;

            owner.transform.localPosition = Vector3.Lerp(startPos, endPos, eased);
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

            obj.transform.position = Vector3.Lerp(startPos, endPos, eased);
        }
    }
}
