using UnityEngine;

namespace QQ.FSM
{
    public class PlayerDieState : IState
    {
        private readonly Actor actor;

        public PlayerDieState(Actor actor)
        {
            this.actor = actor;
        }
        
        public void Enter()
        {
            actor.PlayerMovement.LockMovement();
            actor.SetCanAttack(false);
            actor.SetCurAnimation(AnimState.Die);
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}