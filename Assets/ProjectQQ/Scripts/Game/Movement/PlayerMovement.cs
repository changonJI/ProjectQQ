using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        protected override void OnInit() { }

        protected override void OnStart()
        {
            InputManager.Instance.AddMoveInputEvent(HandleMoveInput);
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveMoveInputEvent(HandleMoveInput);
        }

        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }

        private void HandleMoveInput(Vector2 dir)
        {
            moveDirection = dir;
        }
    }
}