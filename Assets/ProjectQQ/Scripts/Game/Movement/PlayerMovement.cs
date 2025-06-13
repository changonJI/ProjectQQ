using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        protected override void OnInit() { }

        protected override void OnStart()
        {
            InputManager.Instance.OnMoveInput += HandleMoveInput;
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.OnMoveInput -= HandleMoveInput;
        }

        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }

        void HandleMoveInput(Vector2 dir)
        {
            vec2Direction = dir;
        }
    }
}