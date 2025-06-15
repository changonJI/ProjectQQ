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
            actor.SetCanAttack(false);
            actor.SetCurAnimation(AnimState.Die);
            actor.StopForceMove();
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