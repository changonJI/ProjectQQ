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
            txtTitle.text = "타이틀";
            inputField.text = string.Empty;
            txtDefault.text = "입력칸";
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

        public void OnClickConfirm()
        {
            if (inputField.text.Length > limitTxt)
            {
                LogHelper.LogError("글자 초과. 팝업 띄울것. 다시 입력하세요");
            }
            else
            {
                //PlayerPrefs.SetString("NickName", inputField.text);
                Close();
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
    }

}
