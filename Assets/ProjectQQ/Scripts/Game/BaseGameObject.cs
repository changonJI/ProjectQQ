using UnityEngine;

namespace QQ
{
    public interface IBaseGameObjectSet
    {
        public void Init();
    }

    [DisallowMultipleComponent]
    public abstract class BaseGameObject : MonoBehaviour, IBaseGameObjectSet
    {
        public abstract ObjectType Type { get; }

        /// <summary>
        /// BaseGameObject ������ �׻� Init() ����
        /// </summary>
        public BaseGameObject()
        {
            Init();
        }

        public abstract void Init();

        #region ����Ƽ �����ֱ� �Լ�
        protected virtual void Awake()
        {
            OnAwake();
        }
        abstract protected void OnAwake();

        protected virtual void Start()
        {
            OnStart();
        }
        abstract protected void OnStart();

        protected virtual void OnEnable()
        {
            OnEnabled();
        }
        abstract protected void OnEnabled();

        protected virtual void OnDisable()
        {
            OnDisabled();
        }
        abstract protected void OnDisabled();

        protected virtual void Update()
        {
            OnUpdate();
        }
        abstract protected void OnUpdate();

        protected virtual void FixedUpdate()
        {
            OnFixedUpdate();
        }
        abstract protected void OnFixedUpdate();

        protected virtual void LateUpdate()
        {
            OnLateUpdate();
        }
        abstract protected void OnLateUpdate();

        protected virtual void OnDestroy()
        {
            OnDestroyed();
        }
        abstract protected void OnDestroyed();

        #endregion

        #region Ǯ ȣ�� �Լ�
        /// <summary>Ǯ���� ������Ʈ�� ���� �� ȣ��</summary>
        public virtual void GetFromPool()
        {
            Cleanup();          // �̺�Ʈ �ʱ�ȭ, �� �ʱ�ȭ ��
            OnGetFromPool();
        }

        /// <summary>Ǯ�� ������Ʈ�� �ݳ��� �� ȣ��</summary>
        public virtual void ReturnToPool()
        {
            Cleanup();          // �̺�Ʈ �ʱ�ȭ, �� �ʱ�ȭ ��
            OnReturnToPool();   // ���� ���� ��
        }

        /// <summary>������Ʈ�� Ǯ���� ������ �ı��� �� ȣ��</summary>
        public virtual void DestroyFromPool()
        {
            Cleanup();
            OnDestroyFromPool();
        }

        // �ڽ� Ŭ�������� �������̵��� �޼���
        /// <summary> �ʱ�ȭ, �̺�Ʈ ���� ���� �� </summary>
        protected abstract void Cleanup();
        protected abstract void OnGetFromPool();
        protected abstract void OnReturnToPool();
        protected abstract void OnDestroyFromPool();

        #endregion
    }
}