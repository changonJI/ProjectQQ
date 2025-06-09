using UnityEngine.InputSystem;

namespace QQ
{
    public class Actor : BaseGameObject
    {
        public override GameObjectType Type => GameObjectType.Actor;

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
            //TODO : �ӽ� �׽�Ʈ�� �ڵ�. ���Ŀ� FSM, InputManager �߰��Ǹ� ����
            if (Keyboard.current == null) return;
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                LogHelper.LogError("Q ����");
                SetCurAnimation(AnimState.Idle);
            }
            else if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                LogHelper.LogError("w ����");
                SetCurAnimation(AnimState.Roll);
            }
            else if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                LogHelper.LogError("e ����");
                SetCurAnimation(AnimState.Run);
            }
        }
    }
}
