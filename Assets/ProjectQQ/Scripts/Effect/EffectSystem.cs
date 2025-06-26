using UnityEngine;

namespace QQ
{
    public class EffectSystem : BaseGameObject
    {
        public override GameObjectType Type => GameObjectType.SFX;

        // 초기화 변수
        protected float processTime;
        protected Actor owner;

        // 세팅 변수
        protected float duration;
        protected Vector3 startPos;
        protected Vector3 endPos;
        protected float speed;

        protected bool IsFinished => processTime >= duration;

        public override void Init()
        {
            owner = PoolManager.Instance.actor;

            processTime = 0f;
            startPos = owner.transform.localPosition;
        }

        public override void SetData(int id)
        {
            //TODO : SKillTable 세팅

            // Test용
            duration = 1f; // roll animTIme
            speed = 5f;
        }

        #region 유니티 생명주기 함수
        protected override void OnAwake() { }

        protected override void OnStart() { }

        protected override void OnEnabled() { }

        protected override void OnDisabled() { }

        protected override void OnUpdate() { }

        protected override void OnFixedUpdate()
        {
            // 업데이트 작업
            if (IsFinished)
            {
                PoolManager.Instance.ReleaseObject(gameObject);
            }
        }

        protected override void OnLateUpdate() { }

        protected override void OnDestroyed() { }
        #endregion
    }
}