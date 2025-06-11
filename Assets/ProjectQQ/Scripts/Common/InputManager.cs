using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace QQ
{
    public class InputManager : SingletonMono<InputManager>
    {
        private PlayerInputActions inputActions;

        public event Action<Vector2> OnMoveInput;
        public event Action OnRoll;

        private void Awake()
        {
            inputActions = new PlayerInputActions();
            inputActions.Enable();

            // ¿¬°á
            inputActions.Player.Move.performed += ctx => OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
            inputActions.Player.Move.canceled += ctx => OnMoveInput?.Invoke(Vector2.zero);
            inputActions.Player.Roll.performed += ctx => OnRoll?.Invoke();
        }

        private void OnDestroy()
        {
            inputActions.Disable();
        }
    }
}