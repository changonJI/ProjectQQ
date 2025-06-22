using TMPro;
using UnityEngine;

namespace QQ
{
    /// <summary>
    /// �ؽ�Ʈ �Է� �˾�
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
            txtTitle.text = "Ÿ��Ʋ";
            inputField.text = string.Empty;
            txtDefault.text = "�Է�ĭ";
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
            if (inputField.text.Length > limitTxt)
            {
                LogHelper.LogError("���� �ʰ�. �˾� ����. �ٽ� �Է��ϼ���");
            }
            else
            {
                UIPopupConfirm.Instantiate(
                    okAction: CancelNickName,
                    closeAction: ConfirmNickName,
                    "���� �� �� �����ϴ�.", "���", "Ȯ��"
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

            Debug.Log("LoadGameScene");
            LoadingSceneManager.LoadScene(LoadingSceneManager.gameSceneName);
            Close();
        }

        private void CancelNickName()
        {
            UIPopupConfirm.CloseUI();
        }
    }

}
