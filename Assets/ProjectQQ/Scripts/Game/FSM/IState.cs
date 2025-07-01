namespace QQ.FSM
{
    public interface IState
    {
        FSMState GetFSMType();
        void Enter();
        void Update();
        void Exit();
    }
}