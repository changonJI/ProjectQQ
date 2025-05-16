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
            // Ű���� �ƹ� Ű �Է� ���� ��� ��ư ���� (���콺 ����X)
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
