using System;
using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        public Action<Vector2> OnMove;

        protected override void OnInit() { }

        protected override void OnStart()
        {
            InputManager.Instance.AddMoveInputEvent(HandleMoveInput);
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveMoveInputEvent(HandleMoveInput);
        }

        protected override void OnUpdate() {}
        protected override void OnFixedUpdate() {}

        private void HandleMoveInput(Vector2 dir)
        {
            moveDirection = dir;

            if (true == IsMoveBlock)
            {
                return;
            }

            OnMove?.Invoke(dir);
        }
    }
}