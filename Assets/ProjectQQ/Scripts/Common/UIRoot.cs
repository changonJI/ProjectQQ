using System.Collections.Generic;
using UnityEngine;

namespace QQ {
    public sealed class UIRoot : DontDestorySingleton<UIRoot>
    {
        [SerializeField] Canvas canvasHud;
        [SerializeField] Canvas canvasFixed;
        [SerializeField] Canvas canvasOverlay;

        [SerializeField] RectTransform depthHud;
        [SerializeField] RectTransform depthFixed1;
        [SerializeField] RectTransform depthFixed2;
        [SerializeField] RectTransform depthFixed3;
        [SerializeField] RectTransform depthPopup;

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
        }

        void OnDestroy()
        {
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

        public void SetUI(UI ui, GameObject obj, UIDepth depth, int layer)
        {
            SetParent(obj.transform, depth);
            SetLayer(obj, layer);
        }

        public void SetParent(Transform obj, UIDepth depth)
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
            }
        }

        public void SetLayer(GameObject obj, int layer)
        {
            obj.layer = layer; 
            
            foreach (Transform child in obj.transform)
            {
                SetLayer(child.gameObject, layer);
            }
        }

        /// <summary>
        /// ����ȯ�� UI �ʱ�ȭ
        /// </summary>
        public void ClearUI()
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                Destroy(uiList[i]);
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