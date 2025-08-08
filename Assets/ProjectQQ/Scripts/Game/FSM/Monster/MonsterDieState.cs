namespace QQ.FSM
{
    public class MonsterDieState : IState
    {
        public FSMState GetFSMType() => FSMState.Die;

        private readonly Monster monster;

        public bool IsInputBlocked { get; }
        
        private bool hasExited = false;

        public MonsterDieState(Monster monster, MonsterStateContext monsterStateContext)
        {
            this.monster = monster;
        }
        
        public void Enter()
        {
            monster.SetCurAnimation(AnimState.Die);
            monster.SpineAnimator.state.Complete += OnDieAnimationComplete;
        }
        
        private void OnDieAnimationComplete(Spine.TrackEntry trackEntry)
        {
            if (hasExited) return;

            // 현재 완료된 애니메이션이 "die"인지 확인
            if (trackEntry.Animation.Name == monster.GetAnimName(AnimState.Die))
            {
                hasExited = true;

                // 이벤트 제거
                monster.SpineAnimator.AnimationState.Complete -= OnDieAnimationComplete;

                // 상태 종료 및 몬스터 사망 처리
                Exit();
            }
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            monster.Die();
        }
    }
}