using UnityEngine;
using System.Collections.Generic;

namespace QQ
{
    /// <summary>
    /// Component for moving an object
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MovementBase : MonoBehaviour, IOwnable
    {
        [SerializeField] private bool isOnGizmo = false;

        [SerializeField] protected Vector2 moveDirection;
        protected Stack<GridNode> path = new Stack<GridNode>(); // top이 목적지, bottom이 출발점
        protected bool findingPath; // FindPathAsync 작업 걸어두었으면 true

        public Vector2 MoveDirection { get => moveDirection; protected set => moveDirection = value; }
        public Vector2 LastMoveDirection { get; protected set; }
        public bool IsDirectionLock { get; private set; }
        public bool IsMoveBlock { get; private set; }

        public BaseGameObject Owner { get; private set; }
        public float Speed => Owner.GetSpeed();

        public void SetMoveDirectionToLast()
        {
            MoveDirection = LastMoveDirection;
        }

        public void FlipDirection()
        {
            MoveDirection = -MoveDirection;
        }

        public void SetDirectionLock(bool isLock)
        {
            IsDirectionLock = isLock;
        }

        public void SetMoveBlock(bool isBlock, bool clearDirection = false)
        {
            IsMoveBlock = isBlock;

            if (true == isBlock && true == clearDirection)
            {
                MoveDirection = Vector2.zero;
            }
        }

        public bool IsMoving()
        {
            if (true == IsMoveBlock || Vector2.zero == MoveDirection || 0f == Speed)
                return false;

            return true;
        }

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
            if (false == IsMoveBlock && Vector2.zero != MoveDirection)
            {
                Move(MoveDirection, Speed);
            }

            OnFixedUpdate();
        }

        #endregion
        protected virtual void Move(Vector2 dir, float velocity)
        {
            Vector2 vec2DeltaMovement = Time.fixedDeltaTime * velocity * dir;
            Owner.RigidBody.MovePosition(Owner.RigidBody.position + vec2DeltaMovement);

            // refresh last move direction on movement
            LastMoveDirection = dir;
        }

        protected abstract void OnInit();
        protected abstract void OnStart();
        protected abstract void OnDestroyed();
        protected abstract void OnUpdate();
        protected abstract void OnFixedUpdate();
        private void OnDrawGizmos()
        {
            if (true == isOnGizmo && false == findingPath)
            {
                foreach (var pathNode in path)
                {
                    Gizmos.color = UnityEngine.Color.blue;

                    Gizmos.DrawCube(pathNode.WorldPosition, Vector3.one * (Pathfinder.Instance.Grid.NodeDiameter - Pathfinder.Instance.Grid.NodeRadius / 5));
                }
            }
        }
    }
}