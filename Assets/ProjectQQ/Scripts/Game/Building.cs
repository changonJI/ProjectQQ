using UnityEngine;

namespace QQ
{
    public class Building : BaseGameObject
    {
        public override ObjectType Type => ObjectType.Building;

        /// <summary> 생성자 호출 함수 </summary>
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