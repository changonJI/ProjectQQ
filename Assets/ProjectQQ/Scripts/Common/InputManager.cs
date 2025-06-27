using System;
using System.Linq;
using UnityEngine;

namespace QQ
{
    public class InputManager : DontDestroySingleton<InputManager>
    {
        private PlayerInputActions inputActions;
        private InputMap currentInputMap;

        // 게임 플레이 인풋
        private event Action<Vector2> OnMoveInput;
        private event Action OnRollInput;

        // UI 인풋
        private event Action<Vector2> OnUINaviInput;
        private event Action OnUISelectInput;
        private event Action OnUICancelInput;

        protected override void Awake()
        {
            base.Awake();

            inputActions = new PlayerInputActions();
            inputActions.Enable();

            // 게임 플레이 연결
            inputActions.Player.Move.performed += ctx => OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
            inputActions.Player.Move.canceled += ctx => OnMoveInput?.Invoke(Vector2.zero);
            inputActions.Player.Roll.performed += ctx => OnRollInput?.Invoke();

            // UI 연결
            inputActions.UI.Navigate.performed += ctx => OnUINaviInput?.Invoke(ctx.ReadValue<Vector2>());
            inputActions.UI.Select.performed += ctx => OnUISelectInput?.Invoke();
            inputActions.UI.Cancel.performed += ctx => OnUICancelInput?.Invoke();
        }

        protected override void OnDestroy()
        {
            inputActions.Disable();
        }

        public void SwitchInputMap(InputMap context)
        {
            inputActions.Disable();

            switch (context)
            {
                case InputMap.Player:
                    {
                        inputActions.Player.Enable();
                    }
                    break;

                case InputMap.UI:
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
            // action 중복 등록 방지
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
            // action 중복 등록 방지
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
            // action 중복 등록 방지
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
            // action 중복 등록 방지
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
            // action 중복 등록 방지
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