using UnityEngine;

namespace QQ
{
    /// <summary>
    /// Component for moving an object
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MovementBase : MonoBehaviour, IOwnable
    {
        [SerializeField] protected Vector2 moveDirection;
        [SerializeField] protected Vector2 lastMoveDirection;
        public Vector2 MoveDirection => moveDirection;
        public BaseGameObject Owner { get; private set; }

        public bool IsMoveLock { get; private set; } = false;
        public void LockMovement() => IsMoveLock = true;
        public void UnlockMovemnet() => IsMoveLock = false;

        public void Init(BaseGameObject obj)
        {
            Owner = obj;
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
            if (false == IsMoveLock && Vector2.zero != moveDirection)
            {
                Move(moveDirection, Owner.Speed);
            }

            OnFixedUpdate();
        }

        #endregion
        protected virtual void Move(Vector2 dir, float velocity)
        {
            Vector2 vec2DeltaMovement = Time.fixedDeltaTime * velocity * dir;
            Owner.RigidBody.MovePosition(Owner.RigidBody.position + vec2DeltaMovement);

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