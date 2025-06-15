using UnityEngine;
using UnityEngine.InputSystem;

namespace QQ
{
    // TODO
    // * 이동 각도 8방향으로 통제해야 함
    // ~ 고려해야할 것
    // 일시적인 이동속도 증가, 감소 효과(중첩가능) 만들어야할듯
    // 


    /// <summary>
    /// 기본 이동
    /// </summary>
    public abstract class MovementBase : MonoBehaviour
    {
        protected Rigidbody2D rigidBody;

        [SerializeField] protected float baseSpeed = 5f;

        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        public virtual void Move(Vector2 direction)
        {
            if (direction == Vector2.zero) return;

            Vector2 delta = direction.normalized * (baseSpeed * Time.fixedDeltaTime);
            rigidBody.MovePosition(rigidBody.position + delta);
        }
        protected abstract void OnInit();
        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnFixedUpdate();
        protected abstract void OnMoveHandled();
    }
}