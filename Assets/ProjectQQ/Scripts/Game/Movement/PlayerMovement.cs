using System;
using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        public bool isRollStart { get; private set;}
        
        public Action<Vector2> OnMove;
        public Action OnRoll;
        
        // ������ �ӽ� �ڵ�
        private readonly float rollDuration = 0.5f;
        private readonly float rollSpeed = 5f;  // ���ϴ� �� �ӵ�
        private float elapsedTime;
        private Vector2 rollDirection;
        
        protected override void OnInit() { }

        protected override void OnStart()
        {
            InputManager.Instance.AddMoveInputEvent(HandleMoveInput);
            InputManager.Instance.AddRollInputEvent(HandleRollInput);
            
            elapsedTime = 0f; // ������ �ӽ� �ڵ�
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
            // ������ �ӽ� �ڵ�
            elapsedTime += Time.deltaTime;
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
                strDir = "������ ";
            }
            else if (0 > dir.x)
            {
                strDir = "���� ";
            }

            if (0 < dir.y)
            {
                strDir += "��";
            }
            else if (0 > dir.y)
            {
                strDir += "�Ʒ�";
            }

            LogHelper.Log($"������~ ���� {strDir}");
        }
    }
}