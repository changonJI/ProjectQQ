using UnityEngine;

namespace QQ
{
    public interface IBaseGameObjectSet
    {
        public void Init();
    }

    [DisallowMultipleComponent]
    public abstract class BaseGameObject : MonoBehaviour, IBaseGameObjectSet
    {
        public abstract ObjectType Type { get; }

        /// <summary>
        /// BaseGameObject 생성시 항상 Init() 동작
        /// </summary>
        public BaseGameObject()
        {
            Init();
        }

        public abstract void Init();

        #region 유니티 생명주기 함수
        protected virtual void Awake()
        {
            OnAwake();
        }
        abstract protected void OnAwake();

        protected virtual void Start()
        {
            OnStart();
        }
        abstract protected void OnStart();

        protected virtual void OnEnable()
        {
            OnEnabled();
        }
        abstract protected void OnEnabled();

        protected virtual void OnDisable()
        {
            OnDisabled();
        }
        abstract protected void OnDisabled();

        protected virtual void Update()
        {
            OnUpdate();
        }
        abstract protected void OnUpdate();

        protected virtual void FixedUpdate()
        {
            OnFixedUpdate();
        }
        abstract protected void OnFixedUpdate();

        protected virtual void LateUpdate()
        {
            OnLateUpdate();
        }
        abstract protected void OnLateUpdate();

        protected virtual void OnDestroy()
        {
            OnDestroyed();
        }
        abstract protected void OnDestroyed();

        #endregion

        #region 풀 호출 함수
        /// <summary>풀에서 오브젝트를 꺼낼 때 호출</summary>
        public virtual void GetFromPool()
        {
            Cleanup();          // 이벤트 초기화, 값 초기화 등
            OnGetFromPool();
        }

        /// <summary>풀로 오브젝트를 반납할 때 호출</summary>
        public virtual void ReturnToPool()
        {
            Cleanup();          // 이벤트 초기화, 값 초기화 등
            OnReturnToPool();   // 상태 정리 등
        }

        /// <summary>오브젝트가 풀에서 완전히 파괴될 때 호출</summary>
        public virtual void DestroyFromPool()
        {
            Cleanup();
            OnDestroyFromPool();
        }

        // 자식 클래스에서 오버라이드할 메서드
        /// <summary> 초기화, 이벤트 구독 해제 등 </summary>
        protected abstract void Cleanup();
        protected abstract void OnGetFromPool();
        protected abstract void OnReturnToPool();
        protected abstract void OnDestroyFromPool();

        #endregion
    }
}