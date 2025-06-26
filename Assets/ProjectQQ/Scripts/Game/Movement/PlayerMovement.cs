using System;
using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        public bool isRollStart { get; private set;}
        public void SetRollState(bool isRoll) => isRollStart = isRoll;

        public Action<Vector2> OnMove;
        public Action OnRoll;
        
        protected override void OnInit() { }

        protected override void OnStart()
        {
            InputManager.Instance.AddMoveInputEvent(HandleMoveInput);
            InputManager.Instance.AddRollInputEvent(HandleRollInput);
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveMoveInputEvent(HandleMoveInput);
            InputManager.Instance.RemoveRollInputEvent(HandleRollInput);
        }

        protected override void OnUpdate() {}
        protected override void OnFixedUpdate() {}

        private void HandleMoveInput(Vector2 dir)
        {
            if(IsMoveLock) return;
            
            moveDirection = dir;
            OnMove.Invoke(dir);
        }

        public void HandleRollInput()
        {
            if (IsMoveLock ||  isRollStart) return;
           
            EffectManager.Instance.PlayEffect(0).Forget();

            OnRoll?.Invoke();
        }
    }
}