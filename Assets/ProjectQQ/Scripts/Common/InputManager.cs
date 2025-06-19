using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

namespace QQ
{
    public class InputManager : DontDestroySingleton<InputManager>
    {
        private PlayerInputActions inputActions;
        private InputContext currentInputMap;

        // ���� �÷��� ��ǲ
        private event Action<Vector2> OnMoveInput;
        private event Action OnRollInput;

        // UI ��ǲ
        private event Action<Vector2> OnUINaviInput;
        private event Action OnUISelectInput;
        private event Action OnUICancelInput;

        protected override void Awake()
        {
            base.Awake();

            inputActions = new PlayerInputActions();
            inputActions.Enable();

            // ���� �÷��� ����
            inputActions.Player.Move.performed += ctx => OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
            inputActions.Player.Move.canceled += ctx => OnMoveInput?.Invoke(Vector2.zero);
            inputActions.Player.Roll.performed += ctx => OnRollInput?.Invoke();

            // UI ����
            inputActions.UI.Navigate.performed += ctx => OnUINaviInput?.Invoke(ctx.ReadValue<Vector2>());
            inputActions.UI.Select.performed += ctx => OnUISelectInput?.Invoke();
            inputActions.UI.Cancel.performed += ctx => OnUICancelInput?.Invoke();
        }

        private void OnDestroy()
        {
            inputActions.Disable();
        }

        public void SwitchInputMap(InputContext context)
        {
            inputActions.Disable();

            switch (context)
            {
                case InputContext.Player:
                    {
                        inputActions.Player.Enable();
                    }
                    break;

                case InputContext.UI:
                    {
                        inputActions.UI.Enable();
                    }
                    break;
            }

            currentInputMap = context;
        }

        #region PlayerInput
        public void AddMoveInputEvent(Action<Vector2> action)
        {
            // action �ߺ� ��� ����
            if (null == OnMoveInput || false == OnMoveInput.GetInvocationList().Contains(action))
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
            // action �ߺ� ��� ����
            if (null == OnRollInput || false == OnRollInput.GetInvocationList().Contains(action))
            {
                OnRollInput += action;
            }
        }

        public void RemoveRollInputEvent(Action action)
        {
            OnRollInput -= action;
        }
        #endregion

        #region UIInput
        public void AddUINaviInputEvent(Action<Vector2> action)
        {
            // action �ߺ� ��� ����
            if (null == OnUINaviInput || false == OnUINaviInput.GetInvocationList().Contains(action))
            {
                OnUINaviInput += action;
            }
        }

        public void RemoveUINaviInputEvent(Action<Vector2> action)
        {
            OnUINaviInput -= action;
        }

        public void AddUISelectInputEvent(Action action)
        {
            // action �ߺ� ��� ����
            if (null == OnUISelectInput || false == OnUISelectInput.GetInvocationList().Contains(action))
            {
                OnUISelectInput += action;
            }
        }

        public void RemoveUISelectnputEvent(Action action)
        {
            OnUISelectInput -= action;
        }

        public void AddUICancelInputEvent(Action action)
        {
            // action �ߺ� ��� ����
            if (null == OnUICancelInput || false == OnUICancelInput.GetInvocationList().Contains(action))
            {
                OnUICancelInput += action;
            }
        }

        public void RemoveUICancelInputEvent(Action action)
        {
            OnUICancelInput -= action;
        }
        #endregion
    }
}