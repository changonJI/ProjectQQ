using Unity.VisualScripting;
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

            if (collision.CompareTag("Player"))
            {
                var actor = collision.GetComponent<Actor>();
                if (actor != null && !actor.IsDead)
                {
                    actor.TakeDamage(damage, transform.position);
                    lastHitTime = Time.time;
                }
            }
        }
    }
}