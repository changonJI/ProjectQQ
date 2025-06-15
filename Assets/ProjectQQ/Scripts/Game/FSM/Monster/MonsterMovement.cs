using UnityEngine;

namespace QQ.FSM
{
    public class MonsterMovement : MovementBase
    {
        private Vector2 moveDir;

        public void SetDirection(Vector2 dir)
        {
            moveDir = dir;
        }

        public void Tick()
        {
            Move(moveDir);
        }

        protected override void OnInit()
        {
            
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void OnFixedUpdate()
        {
            
        }

        protected override void OnMoveHandled()
        {
            
        }
    }
}