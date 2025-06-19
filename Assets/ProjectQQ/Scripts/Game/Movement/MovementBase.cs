using UnityEngine;

namespace QQ
{
    // TODO
    // * 이동 각도 8방향으로 통제해야 함

    /// <summary>
    /// 기본 이동
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MovementBase : MonoBehaviour
    {
        // [SerializeField] protected Vector2 moveDirection;
        [SerializeField] protected Vector2 lastMoveDirection;

        public Vector2 MoveDirection { get; protected set; }
        protected BaseGameObject owner;

        public void Init(BaseGameObject obj)
        {
            owner = obj;
        }

        #region Unity Method
        protected virtual void Awake()
        {
            OnInit();
        }

        protected virtual void Start()
        {
            OnStart();
        }

        protected virtual void OnDestroy()
        {
            OnDestroyed();
        }

        protected virtual void Update()
        {
            OnUpdate();
        }

        protected virtual void FixedUpdate()
        {
            if (Vector2.zero != MoveDirection)
            {
                Move(MoveDirection, owner.Speed);
            }

            OnFixedUpdate();
        }

        #endregion

        protected virtual void Move(Vector2 dir, float velocity)
        {
            Vector2 vec2DeltaMovement = Time.fixedDeltaTime * velocity * dir;
            owner.RigidBody.MovePosition(owner.RigidBody.position + vec2DeltaMovement);

            lastMoveDirection = MoveDirection;
        }

        public void SetMoveDirection(Vector2 dir)
        {
            MoveDirection = dir;
        }

        protected abstract void OnInit();
        protected abstract void OnStart();
        protected abstract void OnDestroyed();
        protected abstract void OnUpdate();
        protected abstract void OnFixedUpdate();
    }
}