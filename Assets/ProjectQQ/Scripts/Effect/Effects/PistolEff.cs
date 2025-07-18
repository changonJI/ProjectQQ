using UnityEngine;

namespace QQ
{
    public class PistolEff : EffectSystem
    {
        protected override void OnFocus()
        {
            base.OnFocus(); 

            RigidBody.linearVelocity = dir * speed;
        }

        protected override void OnLostFocus()
        {
            base.OnLostFocus();
            RigidBody.linearVelocity = Vector3.zero;
        }
        
        protected override void OnTriggerEnter2Ded(Collider2D other)
        {
            LogHelper.LogError(other.name);
            // Layer 기반 감지
            if (other.gameObject.layer == (int)Layer.Enemy)
            {
                Damage();
            }
        }

        public void Damage()
        {
            if (target == null)
                return;

            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage, transform.position);

                PoolManager.Instance.ReleaseObject(gameObject);
            }
        }

        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
    }
}
