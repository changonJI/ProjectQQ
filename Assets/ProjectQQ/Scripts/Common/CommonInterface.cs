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
        void TakeDamage(int damage, Vector3 lastHitPos);
        void TakeStatus(SkillOptionType type, float value = 0);
    }
    
    public interface ICollectable
    {
        void Collect(Actor actor);
    }
}