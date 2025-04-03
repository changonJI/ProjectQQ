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

            myTransform.SetAsLastSibling(); // 하이라키창 최하단으로 내리기(UI상 맨앞에 오도록)

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
