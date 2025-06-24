using QQ.FSM;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public abstract class BaseGameObject : MonoBehaviour
    {
        public abstract GameObjectType Type { get; }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { isLoaded = value; }
        }

        public virtual float Speed { get; set; }
        public Rigidbody2D RigidBody { get; protected set; }

        #region Status

        protected StatusEffectController status;
        protected bool IsStunned => status.HasStatus(StatusEffectController.StatusEffect.Stunned);

        #endregion

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
            status = new StatusEffectController();

            RigidBody = GetComponent<Rigidbody2D>();
            if (null == RigidBody)
            {
                LogHelper.LogError($"{gameObject.name} 리지드바디2D가 없음");
            }

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