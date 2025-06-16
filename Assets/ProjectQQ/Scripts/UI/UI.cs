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
        public static event System.Action<UI> OnDestroyAction;

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

            SetActive(true);
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

            OnDestroyAction?.Invoke(this);

            OnExit();

            myGameObject = null;
            myTransform = null;
            canvas = null;
        }
        /// <summary>
        /// OnDestroy ���� ���� �޼ҵ�
        /// </summary>
        protected abstract void OnExit();

        public virtual void Close()
        {
            if (uiType == UIType.Back && isActive)
            {
                SetActive(false);
            }
            else if(uiType == UIType.Destroy)
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

        // UI ���� �� �ݹ� �޼ҵ�
        private System.Action OnOkCallback;
        private System.Action OnCloseCallback;
        // UI ���� �� �Ķ����
        private object[] storedParams;

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

        /// <summary>
        ///  UI ���� �޼ҵ�
        /// </summary>
        public static void Instantiate(System.Action okAction = null, 
                                       System.Action closeAction = null,
                                       params object[] args)
        {
            InstantiateUI(okAction, closeAction, args).Forget();
        }

        private static async UniTask<T> InstantiateUI(System.Action okAction, 
                                                       System.Action closeAction, 
                                                       params object[] args)
        {
            if (instance == null)
            {
                await ResManager.Instantiate(typeof(T));
                
                instance.OnOkCallback = okAction;
                instance.OnCloseCallback = closeAction;
                instance.storedParams = args;
            }
                
            instance.SetActive(true);

            return instance;
        }

        public static void CloseUI()
        {
            if (instance == null)
            {
                LogHelper.LogError($"{typeof(T)} instance is null. Cannot close UI.");
                return;
            }

            instance.Close();    
        }

        protected void OnClickOk()
        {
            OnOkCallback?.Invoke();
        }

        protected void OnClickClose()
        {
            OnCloseCallback?.Invoke();
        }

        protected int GetParamToInt(int index)
        {
            if (storedParams.IsValidRange(index))
            {
                return (int)storedParams[index];
            }

            return 0;
        }

        protected float GetParamToFloat(int index)
        {
            if (storedParams.IsValidRange(index))
            {
                return (float)storedParams[index];
            }

            return 0f;
        }

        protected string GetParamToString(int index)
        {
            if (storedParams.IsValidRange(index))
            {
                return storedParams[index].ToString();
            }

            return string.Empty;
        }
    }
}
