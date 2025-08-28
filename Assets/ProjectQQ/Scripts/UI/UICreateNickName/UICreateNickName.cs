using TMPro;
using UnityEngine;

namespace QQ
{
    /// <summary>
    /// 텍스트 입력 팝업
    /// </summary>
    public class UICreateNickName : UI<UICreateNickName>
    {
        public override UIType uiType => UIType.Destroy;

        public override UIDepth uiDepth => UIDepth.Fixed2;

        private const int limitTxt = 8;

        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDefault;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private UIButton btnEnter;

        protected override void OnInit()
        {
            txtTitle.text = 1.ToText();
            inputField.text = string.Empty;
            txtDefault.text = "name";
        }

        protected override void OnStart()
        {
            inputField.onValueChanged.AddListener(OnChanged);
            inputField.onEndEdit.AddListener(OnEndEdit);
            inputField.onSelect.AddListener(OnSelect);
        }

        protected override void OnFocus()
        {
        }

        protected override void OnLostFocus()
        {
        }

        protected override void OnExit()
        {

            inputField.onValueChanged.RemoveListener(OnChanged);
            inputField.onEndEdit.RemoveListener(OnEndEdit);
            inputField.onSelect.RemoveListener(OnSelect);
        }

        public void OnClickConfirm()
        {
            if (inputField.text.Length > limitTxt || inputField.text.TrimEnd() == "")
            {
                LogHelper.LogError("글자 확인 필요");
            }
            else
            {
                UIPopupConfirm.Instantiate(
                    okAction: CancelNickName,
                    closeAction: ConfirmNickName,
                    2.ToText(), 4.ToText() , 3.ToText()
                );
            }
        }

        public void OnChanged(string input)
        {
            if (inputField.text.Length > limitTxt)
            {
                inputField.text = inputField.text.Substring(0, limitTxt);
            }
        }

        public void OnEndEdit(string input)
        {
            OnClickConfirm();
        }

        public void OnSelect(string input)
        {
            inputField.text = string.Empty;
        }

        public void OnDeSelect(string input)
        {
            inputField.text = string.Empty;
        }

        private void ConfirmNickName()
        {
            GameManager.Instance.SavePlayerData(PlayerDataType.UserName, inputField.text);
            UIPopupConfirm.CloseUI();

            GameManager.Instance.LoadScene(SceneType.GameScene, SceneEntryType.EnterStage);
            Close();

        }

        private void CancelNickName()
        {
            UIPopupConfirm.CloseUI();
        }
    }

}
