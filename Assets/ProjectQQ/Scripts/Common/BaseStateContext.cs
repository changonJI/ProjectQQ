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
    }
}