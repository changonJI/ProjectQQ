using System;
using QQ;
using UnityEngine;

namespace ProjectQQ.Scripts.Game.Actions.Player
{
    public class PlayerRangedAttack : MonoBehaviour
    {
        public event Action<bool> OnRangedAttack;

        public float detectRange = 6.0f;
        public LayerMask enemyLayer;
        
        private readonly Collider2D[] hitBuffer = new Collider2D[10];
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

        public void Attack()
        {
            GameObject target = FindClosestEnemy();

            if (target != null)
            {
                Shoot(target.transform);
                OnRangedAttack?.Invoke(true);
            }
            else
            {
                OnRangedAttack?.Invoke(false);
            }
        }

        private GameObject FindClosestEnemy()
        {
            int count = Physics2D.OverlapCircle(transform.position + new Vector3(0, 9, 0), detectRange, filter, hitBuffer);

            float minDist = float.MaxValue;
            GameObject closest = null;

            for (int i = 0; i < count; i++)
            {
                var enemy = hitBuffer[i];
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

        private void Shoot(Transform target)
        {
            LogHelper.Log($"Shoot Projectile To {target.name}");
            // 예: Instantiate(projectilePrefab, actor.transform.position, Quaternion.identity);
            // Projectile.Initialize(target);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnRangedAttack?.Invoke(true);
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 9, 0), detectRange);
        }
#endif
    }
}