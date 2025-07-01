namespace QQ.FSM
{
    public class PlayerStateContext : BaseStateContext
    {
        public IState PlayerIdleState { get; private set; }
        public IState PlayerMoveState { get; private set; }
        public IState PlayerRollState { get; private set; }
        public IState PlayerKnockbackState { get; private set; }
        public IState PlayerDieState { get; private set; }

        public PlayerStateContext(Actor actor)
        {
            PlayerIdleState = new PlayerIdleState(actor, this);
            PlayerMoveState = new PlayerMoveState(actor, this);
            PlayerRollState = new PlayerRollState(actor, this);
            PlayerKnockbackState = new PlayerKnockbackState(actor, this);
            PlayerDieState = new PlayerDieState(actor);
        }

        public override IState GetIdleState() => PlayerIdleState;
        public override IState GetMoveState() => PlayerMoveState;
        public override IState GetRollState() => PlayerRollState;
        public override IState GetKnockbackState() => PlayerKnockbackState;
        public override IState GetDieState() => PlayerDieState;
    }
}