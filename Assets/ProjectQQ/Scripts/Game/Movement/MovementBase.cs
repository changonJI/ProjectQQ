using UnityEngine;

namespace QQ
{
    /// <summary>
    /// Component for moving an object
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MovementBase : MonoBehaviour
    {
        [SerializeField] protected Vector2 moveDirection;
        [SerializeField] protected Vector2 lastMoveDirection;
        protected bool isMoveLock = false;
        public Vector2 MoveDirection => moveDirection;
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
            // only triggers movement if a direction was given
            if (false == isMoveLock && Vector2.zero != moveDirection)
            {
                Move(moveDirection, owner.Speed);
            }

            OnFixedUpdate();
        }

        #endregion
        protected virtual void Move(Vector2 dir, float velocity)
        {
            Vector2 vec2DeltaMovement = Time.fixedDeltaTime * velocity * dir;
            owner.RigidBody.MovePosition(owner.RigidBody.position + vec2DeltaMovement);

            // refresh last move direction on movement
            lastMoveDirection = dir;
        }

        public void SetMoveDirection(Vector2 dir)
        {
            moveDirection = dir;
        }

        protected abstract void OnInit();
        protected abstract void OnStart();
        protected abstract void OnDestroyed();
        protected abstract void OnUpdate();
        protected abstract void OnFixedUpdate();
    }
}