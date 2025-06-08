namespace QQ
{
    public class Actor : BaseGameObject
    {
        public override ObjectType Type => ObjectType.Actor;

        private PlayerStatData playerStatData;

        public override void Init()
        {
            playerStatData = new PlayerStatData();
        }

        public override void SetData(int id)
        {
            var data = PlayerStatDataManager.Instance.Get(id);

            playerStatData.Set(data);
        }

        protected override void OnAwake()
        {
        }

        protected override void OnDestroyed()
        {
        }

        protected override void OnDisabled()
        {
        }

        protected override void OnEnabled()
        {
        }

        protected override void OnFixedUpdate()
        {
        }

        protected override void OnLateUpdate()
        {
        }

        protected override void OnStart()
        {
        }

        protected override void OnUpdate()
        {
        }
    }
}
