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