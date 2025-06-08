using Spine.Unity;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public abstract class BaseGameObject : MonoBehaviour
    {
        public abstract ObjectType Type { get; }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        [SerializeField] private SkeletonAnimation skeletonAnimation;

        /// <summary>
        /// BaseGameObject ������ �׻� Init() ����
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

        public void SetSkeletonAnimation()
        {
        }

        #region ����Ƽ �����ֱ� �Լ�
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