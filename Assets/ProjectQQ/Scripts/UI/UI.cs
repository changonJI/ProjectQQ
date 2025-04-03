using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public abstract class UI<T> : MonoBehaviour where T : UI<T>
    {
        protected static T instance;

        private RectTransform myTransform;
        protected GameObject myGameObject;
        private Canvas canvas;

        protected abstract UIDepth depth { get; }
        public virtual Layer layer => Layer.UI;

        private bool isStart;
        public bool isActive { get; private set; }


        public static async void Instantiate()
        {
            if (instance == null)
            {
                await ResManager.Instantiate(typeof(T));
            }

            instance.SetActive(true);
        }

        protected virtual void Awake()
        {
            instance = this as T;
            myGameObject = gameObject;
            myTransform = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();

            OnInit();
        }

        /// <summary>
        /// awake ���� �޼ҵ�
        /// </summary>
        protected abstract void OnInit();

        protected virtual void Start()
        {
            OnStart();

            isStart = true;
        }

        /// <summary>
        /// start ���� �޼ҵ�
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Instancing ���� OnEnable ����� �ϴ� �޼ҵ�
        /// </summary>
        protected void Focus()
        {
            // ���� �ȵǾ����� return
            if (!isStart)
                return;

            myTransform.SetAsLastSibling(); // ���̶�Űâ ���ϴ����� ������(UI�� �Ǿտ� ������)

            OnFocus();
        }

        /// <summary>
        /// Focus ���� �޼ҵ�
        /// </summary>
        protected abstract void OnFocus();

        /// <summary>
        /// OnDisable ������ �ϴ� �޼ҵ�
        /// </summary>
        protected void LostFocus()
        {
            // ���� �ȵǾ����� return
            if (!isStart)
                return;

            OnLostFocus();
        }

        /// <summary>
        /// LostFocus ���� ���� �޼ҵ�
        /// </summary>
        protected abstract void OnLostFocus();

        protected virtual void OnDestroy()
        {
            LostFocus();

            instance = null;
            myGameObject = null;
            myTransform = null;
            canvas = null;
        }

        public void SetActive(bool isActive)
        {
            if (this.isActive == isActive)
                return;

            this.isActive = isActive;

            if(canvas.enabled != isActive)
                canvas.enabled = isActive;

            if (isActive)
                Focus();
            else
                LostFocus();
        }
    }
}
