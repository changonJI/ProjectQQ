using QQ;
using System;
using UnityEngine;

namespace ProjectQQ.Scripts.Game.Actions.Player
{
    [Obsolete]
    public class PlayerMeleeAttack : MonoBehaviour
    {
        public event Action<bool> OnMeleeEntered;
        
        public LayerMask enemyLayer;
        public float attackRange = 1.5f;
        public int damage = 10;

        private readonly Collider2D[] hitResults = new Collider2D[10]; // GC 방지
        private ContactFilter2D filter;

        private void Awake()
        {
            filter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = enemyLayer,
                useTriggers = true
            };
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Layer 기반 감지
            if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                OnMeleeEntered?.Invoke(true);
            }
        }

        public void Attack()
        {
            var target = FindClosestEnemy();
            if (target == null)
                return;

            LogHelper.Log($"Melee Attack To {target.name}");

            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage, transform.position);
            }
        }

        private GameObject FindClosestEnemy()
        {
            int count = Physics2D.OverlapCircle(transform.position + new Vector3(0, 9, 0), attackRange, filter, hitResults);

            if (count <= 0)
            {
                OnMeleeEntered?.Invoke(false);
                return null;
            }

            float minDist = float.MaxValue;
            GameObject closest = null;

            for (int i = 0; i < count; i++)
            {
                var enemy = hitResults[i];
                if (enemy == null) continue;

                float dist = Vector2.SqrMagnitude(enemy.transform.position - transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemy.gameObject;
                }
            }

            return closest;
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 9, 0), attackRange);
        }
#endif
    }
}