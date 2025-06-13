using UnityEngine;
using UnityEngine.InputSystem;

namespace QQ
{
    // TODO
    // * �̵� ���� 8�������� �����ؾ� ��
    // ~ ����ؾ��� ��
    // �Ͻ����� �̵��ӵ� ����, ���� ȿ��(��ø����) �������ҵ�
    // 


    /// <summary>
    /// �⺻ �̵�
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
                LogHelper.LogError($"MovementBase {gameObject.name} ������ٵ�2D�� ����");
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