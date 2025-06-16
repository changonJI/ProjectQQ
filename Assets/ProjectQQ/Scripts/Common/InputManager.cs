using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

namespace QQ
{
    public class InputManager : DontDestroySingleton<InputManager>
    {
        private PlayerInputActions inputActions;

        private event Action<Vector2> OnMoveInput;
        private event Action OnRollInput;

        protected override void Awake()
        {
            base.Awake();

            inputActions = new PlayerInputActions();
            inputActions.Enable();

            // 연결
            inputActions.Player.Move.performed += ctx => OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
            inputActions.Player.Move.canceled += ctx => OnMoveInput?.Invoke(Vector2.zero);
            inputActions.Player.Roll.performed += ctx => OnRollInput?.Invoke();
        }

        private void OnDestroy()
        {
            inputActions.Disable();
        }

        public void AddMoveInputEvent(Action<Vector2> action)
        {
            // action 중복 등록 방지
            if (null == OnMoveInput || OnMoveInput.GetInvocationList().Contains(action))
            {
                OnMoveInput += action;
            }
        }

        public void RemoveMoveInputEvent(Action<Vector2> action)
        {
            OnMoveInput -= action;
        }

        public void AddRollInputEvent(Action action)
        {
            // action 중복 등록 방지
            if (null == OnRollInput || OnRollInput.GetInvocationList().Contains(action))
            {
                OnRollInput += action;
            }
        }

        public void RemoveRollInputEvent(Action action)
        {
            OnRollInput -= action;
        }
    }
}