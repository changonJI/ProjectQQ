using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QQ
{
    public class UIMainScene : UI<UIMainScene>
    {
        public override UIType uiType => UIType.Main;

        public override UIDepth uiDepth => UIDepth.Fixed1;

        /// <summary>
        /// 키 누르기 전 활성화되어있는 오브젝트
        /// </summary>
        [SerializeField] private GameObject objectActiveBeforeKeyPress;
        /// <summary>
        /// 키 누른 후 활성화시킬 오브젝트
        /// </summary>
        [SerializeField] private GameObject objectActiveAfterKeyPress;
        
        private bool isAnyKeyPressed = false;

        protected override void OnInit()
        {
            objectActiveBeforeKeyPress.SetActive(true);
            objectActiveAfterKeyPress.SetActive(false);
        }

        protected override void OnStart()
        {
        }
        
        protected override void OnFocus()
        {
        }

        protected override void OnLostFocus()
        {
        }

        private void Update()
        {
            if (isAnyKeyPressed) return;

            foreach (var key in Keyboard.current.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    ShowMenu();
                    break;
                }
            }
        }

        public void LoadGameScene()
        {
            Debug.Log("LoadGameScene");
            LoadingSceneManager.LoadScene(LoadingSceneManager.gameSceneName);
        }

        public void ShowMenu()
        {
            isAnyKeyPressed = true;
            objectActiveBeforeKeyPress.SetActive(false);
            objectActiveAfterKeyPress.SetActive(true);
        }
    }
}
