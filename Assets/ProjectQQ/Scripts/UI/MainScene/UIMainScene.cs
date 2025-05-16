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

        protected override void OnInit()
        {
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
            // 키보드 아무 키 입력 받은 경우 버튼 노출 (마우스 대응X)
            if (true == Keyboard.current.anyKey.wasPressedThisFrame)
            {
                ShowMenu();
            }
        }

        public void LoadGameScene()
        {
            LoadingSceneManager.LoadScene(LoadingSceneManager.gameSceneName);
        }

        public void ShowMenu()
        {
            if (default != objectActiveBeforeKeyPress)
            {
                objectActiveBeforeKeyPress.SetActive(false);
            }

            if (default != objectActiveAfterKeyPress)
            {
                objectActiveAfterKeyPress.SetActive(true);
            }
        }
    }
}
