using UnityEngine;

namespace QQ
{
    public class Item : BaseGameObject, ICollectable
    {
        public override GameObjectType Type => GameObjectType.Item;
        protected override void OnInit() { }
        protected override void OnStart() { }
        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnLateUpdate() { }
        protected override void OnDestroyed() { }
        protected override void OnTriggerEnter2Ded(Collider2D other) {}
        protected override void OnFocus() {}
        protected override void OnLostFocus() {}

        public void Collect()
        {
            Debug.Log($"Collect Item : {gameObject.name}");
            Destroy(gameObject);
        }
    }
}