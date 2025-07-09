using System;
using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        [Obsolete]
        public Action<Vector2> OnMove;    // state를 Idle/Move로 바꾸는 방식 event로 처리하게 될 경우 사용

        [SerializeField] protected Vector2 inputDirection;
        public Vector2 InputDirection
        {
            get => inputDirection;
            protected set => inputDirection = value;
        }

        protected override void OnInit() { }

        protected override void OnStart()
        {
            InputManager.Instance.AddMoveInputEvent(HandleMoveInput);
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveMoveInputEvent(HandleMoveInput);
        }

        protected override void OnUpdate()
        {
            if (false == IsDirectionLock && false == IsMoveBlock)
            {
                MoveDirection = InputDirection;
            }
        }
        protected override void OnFixedUpdate() {}

        private void HandleMoveInput(Vector2 dir)
        {
            InputDirection = dir;

            if (true == IsMoveBlock)
            {
                return;
            }
        }
    }
}