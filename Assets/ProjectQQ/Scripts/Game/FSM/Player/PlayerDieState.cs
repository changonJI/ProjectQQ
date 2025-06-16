using UnityEngine;

namespace QQ.FSM
{
    public class PlayerDieState : IState
    {
        private readonly Actor actor;

        public bool IsInputBlocked => true;

        public PlayerDieState(Actor actor)
        {
            this.actor = actor;
        }
        
        public void Enter()
        {
            actor.SetCanAttack(false);
            actor.SetCurAnimation(AnimState.Die);
            actor.PlayerMovement.SetMoveDirection(Vector2.zero);
            // actor.DisableInput(); // 필요 시 입력 차단
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}