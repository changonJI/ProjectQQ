namespace QQ.FSM
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
}