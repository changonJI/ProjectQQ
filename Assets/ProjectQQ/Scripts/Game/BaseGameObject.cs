using QQ.FSM;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public abstract class BaseGameObject : MonoBehaviour
    {
        public abstract GameObjectType Type { get; }

        private bool isActive;
        private bool isStart;
        protected int tableID = 0;

        public virtual float GetSpeed() => 0f;

        public Rigidbody2D RigidBody { get; protected set; }

        #region FSM
        public BaseStateContext stateContext;
        #endregion

        #region Status
        protected StatusEffectController status;
        protected bool IsStunned => status.HasStatus(StatusEffectController.StatusEffect.Stunned);
        #endregion
       
        #region 유니티 생명주기 함수
        protected virtual void Awake()
        {
            status = new StatusEffectController();
            RigidBody = GetComponent<Rigidbody2D>();

            if (null == RigidBody)
            {
                LogHelper.LogError($"{gameObject.name} 리지드바디2D가 없음");
            }
            
            OnInit();
        }

        /// <summary>
        /// 최초 1회 세팅
        /// </summary>
        abstract protected void OnInit();

        protected virtual void Start()
        {
            OnStart();

            isStart = true;

            Focus();
        }
        abstract protected void OnStart();

        /// <summary>
        /// Instancing 이후 OnEnable 기능을 하는 메소드
        /// </summary>
        protected void Focus()
        {
            // 세팅 안되었으면 return
            if (!isStart)
                return;

            OnFocus();
        }

        /// <summary>
        /// Focus 실행 메소드
        /// </summary>
        protected abstract void OnFocus();

        /// <summary>
        /// OnDisable 역할을 하는 메소드
        /// </summary>
        protected void LostFocus()
        {
            // 세팅 안되었으면 return
            if (!isStart)
                return;

            OnLostFocus();
        }

        /// <summary>
        /// LostFocus 이후 실행 메소드
        /// </summary>
        protected abstract void OnLostFocus();

        protected virtual void Update()
        {
            OnUpdate();
        }

        public void SetActive(bool isActive)
        {
            if (this.isActive == isActive)
                return;

            this.isActive = isActive;

            if (gameObject.activeSelf != isActive)
                gameObject.SetActive(isActive);

            if (isActive)
            {
                Focus();
            }
            else
            {
                LostFocus();
            }
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


        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnter2Ded(other);
        }
        abstract protected void OnTriggerEnter2Ded(Collider2D other);
        #endregion

        public void SetParent(Transform transform)
        {
            gameObject.transform.SetParent(transform);
        }

        public void SetTableID(int id)
        {
            tableID = id;

            if(tableID < 0)
            {
                LogHelper.LogError($"{gameObject.name} Table ID 설정 안 됨 : {tableID}");
            }
        }
    }
}