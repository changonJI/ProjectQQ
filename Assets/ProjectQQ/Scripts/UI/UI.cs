using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    /// <summary>
    /// UI 정보 클래스
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasGroup))]
    public abstract class UI : MonoBehaviour
    {
        private RectTransform myTransform;
        private Canvas canvas;
        protected GameObject myGameObject;

        // UIType
        public abstract UIType uiType { get; }
        // UIRoot 위치
        public abstract UIDepth uiDepth { get; }
        // RectTransform Layer
        public virtual Layer layer => Layer.UI;
        // 기본값 false. Init 된 이후 start() 타면 true로 변경
        private bool isStart;   
        // UI 활성화 상태 체크
        public bool isActive { get; private set; }

        // UIRoot 에 추가될때 Action
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
        /// awake 실행 메소드
        /// </summary>
        protected abstract void OnInit();

        protected virtual void Start()
        {
            OnStart();

            isStart = true;
        }

        /// <summary>
        /// start 실행 메소드
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Instancing 이후 OnEnable 기능을 하는 메소드
        /// </summary>
        protected void Focus()
        {
            // 세팅 안되었으면 return
            if (!isStart)
                return;

            // 하이라키창 최하단으로 내리기(UI상 맨앞에 오도록)
            myTransform.SetAsLastSibling(); 

            OnFocus();
        }

        /// <summary>
        /// Focus 실행 메소드
        /// </summary>
        protected abstract void OnFocus();

        /// <summary>
        /// OnDisable 역할을 하는 메소드
        /// </summary>
        protected void LostFocus()
        {
            // 세팅 안되었으면 return
            if (!isStart)
                return;

            OnLostFocus();
        }

        /// <summary>
        /// LostFocus 이후 실행 메소드
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
    /// UI 제네릭 생성 클래스
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
                Debug.LogError($"{typeof(T)} type이 UI에 추가되어 있지 않거나, resource 가 존재하지 않음");
#endif
            instance.SetActive(true);
        }
    }
}
