using UnityEngine;

namespace QQ
{
    public class Character : BaseGameObject
    {
        public override ObjectType Type => ObjectType.Monster;

        /// <summary> ������ ȣ�� �Լ� </summary>
        public override void Init()
        {

        }

        protected override void OnAwake() { }
        protected override void OnStart() { }
        protected override void OnEnabled() { }
        protected override void OnDisabled() { }
        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnLateUpdate() { }
        protected override void OnDestroyed() { }



        protected override void Cleanup() { }
        protected override void OnGetFromPool() { }
        protected override void OnReturnToPool() { }
        protected override void OnDestroyFromPool() { }
    }
}