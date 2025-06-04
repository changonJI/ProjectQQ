using UnityEngine;

namespace QQ
{
    public class Item : BaseGameObject
    {
        public override ObjectType Type => ObjectType.Item;

        /// <summary> 생성자 호출 함수 </summary>
        public override void Init()
        {

        }

        public override void SetData(int id)
        {
            // 데이터테이블에서 id에 해당하는 데이터 얻어오기
            { }

            // 데이터 오브젝트에 세팅
            { }
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