namespace QQ.FSM
{
    public class PlayerDieState : IState
    {
        public FSMState GetFSMType() => FSMState.Die;
        private readonly Actor actor;

        public PlayerDieState(Actor actor)
        {
            this.actor = actor;
        }
        
        public void Enter()
        {
            actor.PlayerMovement.SetMoveBlock(true, true);
            actor.SetCanRangedAttack(false);
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