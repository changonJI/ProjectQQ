using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    /// <summary>
    /// UI ���� Ŭ����
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup))]
    public abstract class UI : MonoBehaviour
    {
        private RectTransform myTransform;
        private Canvas canvas;
        protected GameObject myGameObject;

        // UIType
        public abstract UIType uiType { get; }
        // UIRoot ��ġ
        public abstract UIDepth uiDepth { get; }
        // RectTransform Layer
        public virtual Layer layer => Layer.UI;
        // �⺻�� false. Init �� ���� start() Ÿ�� true�� ����
        private bool isStart;   
        // UI Ȱ��ȭ ���� üũ
        public bool isActive { get; private set; }

        // UIRoot �� �߰��ɶ� Action
        public static event System.Action<UI> OnCreateAction;
        public static event System.Action<UI> OnFocusAction;
        public static event System.Action<UI> OnLostFocusAction;

        protected virtual void Awake()
        {
            myGameObject = gameObject;
            myTransform = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();

            OnCreateAction?.Invoke(this);

            UIRoot.Instance.SetUI(this, myGameObject, uiDepth, (int)layer);

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

            // ���̶�Űâ ���ϴ����� ������(UI�� �Ǿտ� ������)
            myTransform.SetAsLastSibling(); 

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

            myGameObject = null;
            myTransform = null;
            canvas = null;
        }

        public virtual void Close()
        {
            if (uiType == UIType.Back && isActive)
            {
                SetActive(false);
            }
            else if(uiType == UIType.Destroy && isActive)
            {
                SetActive(false);
                Destroy(this.gameObject);
            }
            else if(uiType == UIType.Main)
            {
                return;
            }
        }

        public void SetActive(bool isActive)
        {
            if (this.isActive == isActive)
                return;

            this.isActive = isActive;

            if(canvas.enabled != isActive)
                canvas.enabled = isActive;

            if (isActive)
            {
                Focus();
                OnFocusAction?.Invoke(this);
            }
            else
            {
                LostFocus();
                OnLostFocusAction?.Invoke(this);
            }

        }
    }

    /// <summary>
    /// UI ���׸� ���� Ŭ����
    /// </summary>
    public abstract class UI<T> : UI where T : UI<T>
    {
        protected static T instance;

        protected override void Awake()
        {
            base.Awake();

            instance = this as T;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            instance = null;
        }

        public static void Instantiate()
        {
            InstantiateUI().Forget();
        }

        private static async UniTaskVoid InstantiateUI()
        {
            if (instance == null)
            {
                await ResManager.Instantiate(typeof(T));
            }

#if UNITY_EDITOR
            if (instance == null)
                Debug.LogError($"{typeof(T)} type�� UI�� �߰��Ǿ� ���� �ʰų�, resource �� �������� ����");
#endif
            instance.SetActive(true);
        }
    }
}
