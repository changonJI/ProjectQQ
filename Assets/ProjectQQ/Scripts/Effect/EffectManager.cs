using Cysharp.Threading.Tasks;

namespace QQ
{
    public class EffectManager : DontDestroySingleton<EffectManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void Init() { }

        public async UniTaskVoid PlayEffect(int skillID)
        {
            // TODO : GET SKillTable Data
            if (PoolManager.IsValid())
            {
                await PoolManager.Instance.GetObject(GameObjectType.SFX, "RollEff", 0);
            }
        }
    }
}