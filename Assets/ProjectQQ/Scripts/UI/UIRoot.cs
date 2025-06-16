using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace QQ {
    public sealed class UIRoot : DontDestroySingleton<UIRoot>
    {
        [SerializeField] Canvas canvasHud;
        [SerializeField] Canvas canvasFixed;
        [SerializeField] Canvas canvasOverlay;

        [SerializeField] RectTransform depthHud;
        [SerializeField] RectTransform depthFixed1;
        [SerializeField] RectTransform depthFixed2;
        [SerializeField] RectTransform depthFixed3;
        [SerializeField] RectTransform depthPopup;
        [SerializeField] RectTransform depthToast;
        [SerializeField] RectTransform depthIndicator;

        private List<UI> uiList;
        private List<UI> escapeUIList;

        protected override void Awake()
        {
            base.Awake();

            uiList = new List<UI>();
            escapeUIList = new List<UI>();

            UI.OnCreateAction += OnCreateAction;
            UI.OnFocusAction += OnFocusAction;
            UI.OnLostFocusAction += OnLostFocusAction;
            UI.OnDestroyAction += OnDestroyAction;
        }

        void OnDestroy()
        {
            UI.OnCreateAction -= OnCreateAction;
            UI.OnFocusAction -= OnFocusAction;
            UI.OnLostFocusAction -= OnLostFocusAction;
            UI.OnDestroyAction -= OnDestroyAction;

            depthHud = null;
            depthFixed1 = null;
            depthFixed2= null;
            depthFixed3= null;
            depthPopup = null;

            uiList = null;
            escapeUIList = null;
        }

        private void Update()
        {
            
        }

        public void OnCreateAction(UI ui)
        {
            //NOTE: Indicator는 uiList에서 제외. 추가시 씬전환할때 ClearUI에서 충돌
            if (ui.uiDepth == UIDepth.Indicator)
                return;

            uiList.Add(ui);
        }

        private void OnFocusAction(UI ui)
        {
            if (ui.uiType == UIType.Back)
                escapeUIList.Add(ui);
        }

        private void OnLostFocusAction(UI ui)
        {
            if (ui.uiType == UIType.Back)
                escapeUIList.Remove(ui);
        }

        public void OnDestroyAction(UI ui)
        {
            if(uiList.Contains(ui))
                uiList.Remove(ui);
        }

        public void SetUI(UI ui, GameObject obj, UIDepth depth, int layer)
        {
            SetParent(obj.transform, depth);
            SetLayer(obj, layer);
            SetOffset(obj.GetComponent<RectTransform>());
        }

        private void SetParent(Transform obj, UIDepth depth)
        {
            switch (depth)
            {
                case UIDepth.HUD:
                    obj.SetParent(depthHud);
                    break;
                case UIDepth.Fixed1:
                    obj.SetParent(depthFixed1);
                    break;
                case UIDepth.Fixed2:
                    obj.SetParent(depthFixed2);
                    break;
                case UIDepth.Fixed3:
                    obj.SetParent(depthFixed3);
                    break;
                case UIDepth.Popup:
                    obj.SetParent(depthPopup);
                    break;
                case UIDepth.Toast:
                    obj.SetParent(depthToast);
                    break;
                case UIDepth.Indicator:
                    obj.SetParent(depthIndicator);
                    break;
            }            
        }

        private void SetLayer(GameObject obj, int layer)
        {
            obj.layer = layer; 
            
            foreach (Transform child in obj.transform)
            {
                SetLayer(child.gameObject, layer);
            }
        }

        private void SetOffset(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// 씬전환시 UI 초기화
        /// </summary>
        public async UniTask ClearUI()
        {
            for(int i = 0; i < uiList.Count; i++)
            {                
                if (uiList[i] != null)
                    Destroy(uiList[i].gameObject);

                await UniTask.Yield();
            }

            uiList.Clear();
        }

        public void EscapeEvent(UI ui)
        {
            for (int i = 0; i < escapeUIList.Count; i++)
            {
                if (escapeUIList[i] == ui)
                {
                    escapeUIList[i].Close();
                    return;
                }
            }

            ApplcationQuit();
        }


        private void ApplcationQuit()
        {
            Application.Quit();
        }
    }
}