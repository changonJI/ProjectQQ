using UnityEngine;

namespace QQ
{
    public interface IOwnable
    {
        public void Init(BaseGameObject ownerObj);
        public BaseGameObject Owner { get; }
    }
    
    public interface IDamageable
    {
        void TakeDamage(int damage);
    }
    
    public interface ICollectible
    {
        void Collect();
    }
}