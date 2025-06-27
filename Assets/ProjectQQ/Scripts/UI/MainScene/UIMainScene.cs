using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QQ
{
    public class UIMainScene : UI<UIMainScene>
    {
        public override UIType uiType => UIType.Main;

        public override UIDepth uiDepth => UIDepth.Fixed1;

        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private UIButtonAndText btnNewGame;
        [SerializeField] private UIButtonAndText btnContinue;
        [SerializeField] private UIButtonAndText btnTraining;
        [SerializeField] private UIButtonAndText btnSetting;
        [SerializeField] private UIButtonAndText btnExit;

        protected override void OnInit()
        {
            btnNewGame.OnClickClear();
            btnContinue.OnClickClear();
            btnTraining.OnClickClear();
            btnSetting.OnClickClear();
            btnExit.OnClickClear();

            txtTitle.text = string.Empty;
        }

        protected override void OnStart()
        {
            btnNewGame.OnClickAdd(OnClickNewGame);
            btnNewGame.SetText("Test");
            btnContinue.OnClickAdd(OnClickContinue);
            btnContinue.SetText("Test");
            btnTraining.OnClickAdd(OnClickTraining);
            btnTraining.SetText("Test");
            btnSetting.OnClickAdd(OnClickSetting);
            btnSetting.SetText("Test");
            btnExit.OnClickAdd(OnClickExit);
            btnExit.SetText("Exit Game");

            txtTitle.text = "Project QQ_test";

            Init();
        }

        protected override void OnFocus()
        {
        }

        protected override void OnLostFocus()
        {
        }

        protected override void OnExit()
        {
            btnNewGame.OnClickRemove(OnClickNewGame);
            btnNewGame.SetText(string.Empty);
            btnContinue.OnClickRemove(OnClickContinue);
            btnContinue.SetText(string.Empty);
            btnTraining.OnClickRemove(OnClickTraining);
            btnTraining.SetText(string.Empty);
            btnSetting.OnClickRemove(OnClickSetting);
            btnSetting.SetText(string.Empty);
            btnExit.OnClickRemove(OnClickExit);
            btnExit.SetText(string.Empty);

            txtTitle.text = string.Empty;
        }

        private void Update()
        {
            foreach (var key in Keyboard.current.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    break;
                }
            }
        }

        private void Init()
        {
            if (GameManager.Instance.GetStringPlayerData(PlayerDataType.UserName) == string.Empty)
            {
                UICreateNickName.Instantiate();
            }
        }

        private void OnClickNewGame()
        {
            Debug.Log("LoadGameScene");
            GameManager.Instance.LoadScene(SceneType.GameScene);
        }

        private void OnClickContinue()
        {
            Debug.Log("Continue");
        }

        private void OnClickTraining()
        {
            Debug.Log("Training");
        }

        private void OnClickSetting()
        {
            Debug.Log("Setting");
        }

        private void OnClickExit()
        {
            Debug.Log("Exit Game");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
