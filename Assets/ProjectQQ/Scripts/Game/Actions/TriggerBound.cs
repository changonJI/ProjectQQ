using System;
using UnityEngine;

namespace QQ.Action
{
    public abstract class TriggerBound : MonoBehaviour
    {
        public CircleCollider2D Collider2D;
        
        public abstract void OnTriggerEnter2D(Collider2D other);
        public abstract void OnTriggerExit2D(Collider2D other);
    }
}