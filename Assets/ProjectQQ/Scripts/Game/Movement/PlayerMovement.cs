using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        private bool isRollStart = false;
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
        protected override void OnFixedUpdate()
        {
            if (true == isRollStart)
            {
                Roll(lastMoveDirection);
                isRollStart = false;
            }
        }

        private void HandleMoveInput(Vector2 dir)
        {
            moveDirection = dir;
        }
        private void HandleRollInput()
        {
            isRollStart = true;
        }

        private void Roll(Vector2 dir)
        {
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