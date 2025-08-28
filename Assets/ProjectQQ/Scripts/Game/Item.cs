using UnityEngine;

namespace QQ
{
    public enum OriginTypeItem
    {
        Pistol = 3,
        Rifle = 5,
        Grenade = 8,
        Dagger = 13,
        Flashbang = 18,
        Camera = 23,
        Cigarette = 28,
        EnergyDrink = 33
    }
    public class Item : BaseGameObject, ICollectable
    {
        public override GameObjectType Type => GameObjectType.Item;
        protected override void OnInit() 
        {
            SetLayer(gameObject, GameObjectType.Item);
        }

        protected override void OnStart() { }
        protected override void OnUpdate() { }
        protected override void OnFixedUpdate() { }
        protected override void OnLateUpdate() { }
        protected override void OnDestroyed() { }
        protected override void OnFocus() {}
        protected override void OnLostFocus() {}
        protected override void OnTriggerEnter2Ded(Collider2D other) {}
        protected override void OnCollisionEnter2Ded(Collision2D other) { }

        protected virtual void OnUseItem(Actor actor) {}

        public void Collect(Actor actor)
        {
            Debug.Log($"Collect Item : {gameObject.name}");
            
            OnUseItem(actor);
        }

        
    }
}