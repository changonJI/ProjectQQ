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
            //TODO : 임시 테스트용 코드. 추후에 FSM, InputManager 추가되면 제거
            if (Keyboard.current == null) return;
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                LogHelper.LogError("Q 눌름");
                SetCurAnimation(AnimState.Idle);
            }
            else if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                LogHelper.LogError("w 눌름");
                SetCurAnimation(AnimState.Roll);
            }
            else if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                LogHelper.LogError("e 눌름");
                SetCurAnimation(AnimState.Run);
            }
        }
    }
}
