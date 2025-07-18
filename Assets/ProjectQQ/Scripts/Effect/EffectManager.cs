using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    public class EffectManager : DontDestroySingleton<EffectManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void Init() { }

        public async UniTaskVoid PlayEffect(int skillID)
        {
            // TODO : GET SKillTable Data
            if (PoolManager.IsValid())
            {
                var effect = await PoolManager.Instance.GetObject(GameObjectType.SFX, "PistolEff", 0);
            }
        }

        /// <summary>
        /// 내적, Cos값을 이용한 각도 구하기
        /// cosTheta = ((x1 * x2) + (y1 * y2)) / (vec3A.magnitude * vec3B.magnitude);
        /// </summary>
        public void SetAngle(Transform effect, Transform owner, Transform target)
        {
            // 바라보는 방향 정규화값
            Vector3 dir = (target.transform.localPosition - owner.transform.localPosition).normalized;
            // 0도 기준 내적값(내적 == cosTheta 값)
            float cosTheta = Vector3.Dot(Vector3.up, dir);
            // cosTheta값으로 Theta값 구하기. Acos은 radian 값이므로 rad2deg를 곱해준다.
            float theta = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

            // 시계방향으로 돌리기위해 -값을 곱해준다.
            effect.localRotation = Quaternion.Euler(0, 0, -theta);

            LogHelper.DrawLine(owner.localPosition, target.localPosition, Color.red, 1f);
        }
    }
}