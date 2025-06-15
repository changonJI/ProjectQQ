using QQ.FSM;
using UnityEngine;

namespace QQ
{
    public class PlayerMovement : MovementBase
    {
        protected override void OnInit() { }
        protected override void OnStart() { }
        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnMoveHandled() { }
        private Vector2? overrideVelocity = null;
        
        public void SetOverrideVelocity(Vector2 velocity)
        {
            overrideVelocity = velocity;
        }

        public void ClearOverrideVelocity()
        {
            overrideVelocity = null;
        }

        public override void Move(Vector2 direction)
        {
            Vector2 moveVec;
            
            if (overrideVelocity != null)
            {
                moveVec = overrideVelocity.Value * Time.fixedDeltaTime;
            }
            else
            {
                moveVec = direction.normalized * (baseSpeed * Time.fixedDeltaTime);
            }

            rigidBody.MovePosition(rigidBody.position + moveVec);
        }
    }
}