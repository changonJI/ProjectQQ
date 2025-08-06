using QQ.FSM;
using UnityEngine;

namespace QQ
{
    public class Monster : SpineGameObject, IDamageable
    {
        public override GameObjectType Type => GameObjectType.Monster;

        private MonsterData monsterData;
        
        public MonsterMovement MonsterMovement { get; private set; }
        
        private StatusEffectController.StatusEffect currentStatus = StatusEffectController.StatusEffect.None;

        // 이속
        public override float GetSpeed() => monsterData.speed + addSpeed;
        private float addSpeed = 0f;
        public Transform TargetTransform { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            InitMonster();
            InitController();
        }

        protected override void OnStart()
        {
            SetTable();
            stateContext.ChangeState(stateContext.GetIdleState());

            TryFindPlayer();
        }

        protected override void OnUpdate()
        {
            TryFindPlayer();
            stateContext.Update();
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

        public void TakeDamage(int damage, Vector3 transformPosition)
        {
            Debug.Log("아얏");
        }

        private void InitMonster()
        {
            monsterData = new MonsterData();
            stateContext = new MonsterStateContext(this);
            
        }

        private void InitController()
        {
            MonsterMovement = gameObject.AddComponent<MonsterMovement>(this);
        }

        private void SetTable()
        {
            var data = MonsterDataManager.Instance.Get(tableID);

            monsterData.Set(data);
        }
    }
}