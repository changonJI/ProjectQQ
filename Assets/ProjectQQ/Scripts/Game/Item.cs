using UnityEngine;

namespace QQ
{
    public class Item : BaseGameObject
    {
        public override ObjectType Type => ObjectType.Item;

        /// <summary> ������ ȣ�� �Լ� </summary>
        public override void Init()
        {

        }

        public override void SetData(int id)
        {
            // ���������̺��� id�� �ش��ϴ� ������ ������
            { }

            // ������ ������Ʈ�� ����
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