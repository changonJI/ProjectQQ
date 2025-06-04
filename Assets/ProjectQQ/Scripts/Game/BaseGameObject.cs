using UnityEngine;

namespace QQ
{
    public interface IBaseGameObjectSet
    {
        public void Init();

        public void SetData(int id);
    }

    [DisallowMultipleComponent]
    public abstract class BaseGameObject : MonoBehaviour, IBaseGameObjectSet
    {
        public abstract ObjectType Type { get; }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        /// <summary>
        /// BaseGameObject 생성시 항상 Init() 동작
        /// </summary>
        public BaseGameObject()
        {
            Init();
        }

        public abstract void Init();
        public abstract void SetData(int id);
        public void SetParent(Transform transform)
        {
            gameObject.transform.SetParent(transform);
        }

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
    }
}