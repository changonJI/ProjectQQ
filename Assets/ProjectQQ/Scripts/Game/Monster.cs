using QQ.FSM;
using UnityEngine;

namespace QQ
{
    public class Monster : SpineGameObject
    {
        public override GameObjectType Type => GameObjectType.Monster;

        private MonsterData monsterData;
        
        public MonsterMovement MonsterMovement { get; private set; }
        public MonsterStateContext StateContext { get; private set; }
        
        private StatusEffectController.StatusEffect currentStatus = StatusEffectController.StatusEffect.None;
        
        public Transform TargetTransform { get; private set; }

        /// <summary> 생성자 호출 함수 </summary>
        public override void Init()
        {
            monsterData = new MonsterData();
        }

        public override void SetData(int id)
        {
            var data = MonsterDataManager.Instance.Get(id);

            monsterData.Set(data);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            StateContext = new MonsterStateContext(this);
            MonsterMovement = GetComponent<MonsterMovement>();
        }

        protected override void OnStart()
        {
            StateContext.ChangeState(StateContext.MonsterIdleState);
            TryFindPlayer();
        }

        protected override void OnEnabled() { }
        protected override void OnDisabled() { }

        protected override void OnUpdate()
        {
            TryFindPlayer();
            StateContext.Update();
        }

        protected override void OnFixedUpdate()
        {
        }
        protected override void OnLateUpdate() { }
        protected override void OnDestroyed() { }

        public void TryFindPlayer()
        {
            if (TargetTransform == null)
            {
                var player = PoolManager.Instance.actor;
                if (player != null) //  && !player.IsDead
                {
                    TargetTransform = player.transform;
                }
            }
        }
    }
}