using UnityEngine;

namespace QQ
{
    public class Building : BaseGameObject
    {
        public override GameObjectType ObjType => GameObjectType.Building;

        protected override void OnInit() { }
        protected override void OnStart() { }
        protected override void OnFocus() {}
        protected override void OnLostFocus() {}
        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnLateUpdate() { }
        protected override void OnDestroyed() { }

        protected override void OnTriggerEnter2Ded(Collider2D other) {}

        protected override void OnCollisionEnter2Ded(Collision2D other) {}
    }
}