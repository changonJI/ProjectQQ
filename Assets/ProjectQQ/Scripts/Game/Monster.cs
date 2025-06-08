namespace QQ
{
    public class Monster : BaseGameObject
    {
        public override ObjectType Type => ObjectType.Monster;

        private MonsterData monsterData;

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

        protected override void OnAwake() { }
        protected override void OnStart() { }
        protected override void OnEnabled() { }
        protected override void OnDisabled() { }
        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnLateUpdate() { }
        protected override void OnDestroyed() { }
    }
}