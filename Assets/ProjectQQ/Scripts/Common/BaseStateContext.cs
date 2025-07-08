using QQ.FSM;
using System.Collections.Generic;

namespace QQ
{
    public abstract class BaseStateContext
    {
        // 각 상태(State)에서 전이(Transition)할 수 있는 상태 목록 정의
        public static readonly Dictionary<FSMState, HashSet<FSMState>> AllowedTransrations = new()
        {
            { FSMState.Idle,        new HashSet<FSMState> { FSMState.Move, FSMState.Roll, FSMState.Knockback, FSMState.Die } },
            { FSMState.Move,        new HashSet<FSMState> { FSMState.Idle, FSMState.Roll, FSMState.Knockback, FSMState.Die } },
            { FSMState.Roll,        new HashSet<FSMState> { FSMState.Idle, FSMState.Move, FSMState.Die} },
            { FSMState.Knockback,   new HashSet<FSMState> { FSMState.Idle, FSMState.Move, FSMState.Roll, FSMState.Die } },
            { FSMState.Die,         new HashSet<FSMState> { FSMState.Idle } }
        };

        private IState currentState;

        public void ChangeState(IState newState)
        {
            if (newState == currentState)
                return;

            if (false == AllowedTransrations[currentState.GetFSMType()].Contains(newState.GetFSMType()))
                return;

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