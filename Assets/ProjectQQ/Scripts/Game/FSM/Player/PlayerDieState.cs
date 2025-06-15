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
            // actor.DisableInput(); // �ʿ� �� �Է� ����
        }

        public void Update()
        {
        }

        public void Exit()
        {
        }
    }
}