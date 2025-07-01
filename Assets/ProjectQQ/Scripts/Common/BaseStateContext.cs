using QQ.FSM;

namespace QQ
{
    public abstract class BaseStateContext
    {
        private IState currentState;
        public IState CurrentState => currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public virtual FSMState GetCurFSMType() => currentState?.GetFSMType() ?? FSMState.None;
        public virtual IState GetIdleState() => null;
        public virtual IState GetMoveState() => null;
        public virtual IState GetRollState() => null;
        public virtual IState GetKnockbackState() => null;
        public virtual IState GetDieState() => null;
    }
}