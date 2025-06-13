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
    [DisallowMultipleComponent]
    public abstract class MovementBase : MonoBehaviour
    {
        private Rigidbody2D rigidBody;
        [SerializeField] protected float speedBase;
        [SerializeField] protected Vector2 vec2Direction;

        public float Velocity => speedBase;

        #region Unity Method
        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            if (null == rigidBody)
            {
                LogHelper.LogError($"MovementBase {gameObject.name} 리지드바디2D가 없음");
            }

            OnInit();
        }

        protected virtual void Start()
        {
            OnStart();
        }

        protected virtual void OnDestroy()
        {
            OnDestroyed();
        }

        protected virtual void Update()
        {
            OnUpdate();
        }

        protected virtual void FixedUpdate()
        {
            Move(vec2Direction, Velocity);

            OnFixedUpdate();
        }

        #endregion

        protected virtual void Move(Vector2 dir, float velocity)
        {
            Vector2 vec2DeltaMovement = dir * velocity * Time.fixedDeltaTime;
            rigidBody.MovePosition(rigidBody.position + vec2DeltaMovement);
        }

        protected abstract void OnInit();
        protected abstract void OnStart();
        protected abstract void OnDestroyed();
        protected abstract void OnUpdate();
        protected abstract void OnFixedUpdate();
    }
}