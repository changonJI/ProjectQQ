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
        void TakeDamage(int damage, Vector3 transformPosition);
    }
    
    public interface ICollectable
    {
        void Collect();
    }
}