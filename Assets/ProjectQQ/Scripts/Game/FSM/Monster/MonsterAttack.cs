using UnityEngine;

namespace QQ.FSM
{
    public class MonsterAttack : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private float hitCooldown = 1f;

        private float lastHitTime;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(Time.time - lastHitTime < hitCooldown) return;

            if (collision.TryGetComponent<IDamageable>(out var damageable) && collision.CompareTag("Player"))
            {
                damageable.TakeDamage(damage, transform.position);
            }
        }
    }
}