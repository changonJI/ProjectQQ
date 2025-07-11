using System;
using QQ;
using UnityEngine;

namespace ProjectQQ.Scripts.Game.Actions.Player
{
    public class PlayerItemCollector : MonoBehaviour
    {
        public event Action<bool> OnItemCollected; 
        public float pickupRange = 1.0f;
        public LayerMask itemLayer;

        private Collider2D[] itemResults = new Collider2D[10];

        public void TryCollectItems()
        {
            Vector2 center = transform.position + new Vector3(0, 9, 0);

            int count = Physics2D.OverlapCircleNonAlloc(center, pickupRange, itemResults, itemLayer);

            if (count <= 0)
            {
                OnItemCollected?.Invoke(false);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Collider2D col = itemResults[i];

                if (col.TryGetComponent<ICollectable>(out var item))
                {
                    item.Collect();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, 9, 0), pickupRange);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnItemCollected?.Invoke(true);
        }
    }
}