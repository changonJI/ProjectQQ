using System;
using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        public bool isRollStart { get; private set;}
        
        public Action<Vector2> OnMove;
        public Action OnRoll;
        
        // 구르기 임시 코드
        private readonly float rollDuration = 0.5f;
        private readonly float rollSpeed = 5f;  // 원하는 롤 속도
        private float elapsedTime;
        private Vector2 rollDirection;
        
        protected override void OnInit() { }

        protected override void OnStart()
        {
            InputManager.Instance.AddMoveInputEvent(HandleMoveInput);
            InputManager.Instance.AddRollInputEvent(HandleRollInput);
            
            elapsedTime = 0f; // 구르기 임시 코드
        }

        protected override void OnDestroyed()
        {
            InputManager.Instance.RemoveMoveInputEvent(HandleMoveInput);
            InputManager.Instance.RemoveRollInputEvent(HandleRollInput);
        }

        protected override void OnUpdate() { }
        protected override void OnFixedUpdate()
        {
            if (true == isRollStart)
            {
                Roll(lastMoveDirection);
            }
        }

        private void HandleMoveInput(Vector2 dir)
        {
            if(IsMoveLock) return;
            
            moveDirection = dir;
            OnMove.Invoke(dir);
        }

        public void HandleRollInput()
        {
            if (IsMoveLock ||  isRollStart) return;
            
            isRollStart = true;
            OnRoll?.Invoke();
        }

        private void Roll(Vector2 dir)
        {
            // 구르기 임시 코드
            elapsedTime += Time.deltaTime;
            EffectManager.PlayEffect(Owner.gameObject, EffectManager.EffectType.Roll, rollSpeed);
            if (elapsedTime >= rollDuration)
            {
                isRollStart = false;
                elapsedTime = 0f;
                return;
            }
            
            
            // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            string strDir = "";
            if (0 < dir.x)
            {
                strDir = "오른쪽 ";
            }
            else if (0 > dir.x)
            {
                strDir = "왼쪽 ";
            }

            if (0 < dir.y)
            {
                strDir += "위";
            }
            else if (0 > dir.y)
            {
                strDir += "아래";
            }

            LogHelper.Log($"데구룽~ 방향 {strDir}");
        }
    }
}