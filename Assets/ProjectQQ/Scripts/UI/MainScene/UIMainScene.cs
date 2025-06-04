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
        /// Ű ������ �� Ȱ��ȭ�Ǿ��ִ� ������Ʈ
        /// </summary>
        [SerializeField] private GameObject objectActiveBeforeKeyPress;
        /// <summary>
        /// Ű ���� �� Ȱ��ȭ��ų ������Ʈ
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
