using System;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace QQ
{
    /// <summary>
    /// �ؽ�Ʈ �Է� �˾�
    /// </summary>
    public class UIPopupPrompt : UIPopup<UIPopupPrompt>
    {
        [SerializeField] private TextMeshProUGUI promptTextTMP;
        [SerializeField] private TextMeshProUGUI placeholderTextTMP;
        [SerializeField] private TMP_Text inputTextTMP;
        [SerializeField] private UIButton btnEnter;
        
        protected override void OnInit()
        {
            base.OnInit();
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

        /// <summary>
        /// �ؽ�Ʈ �Է� �˾� ��ư ���� ����
        /// </summary>
        /// <param name="onOk">Ȯ�� ��ư OnClick ����</param>
        public void SetBtnAction(Action<string> onOk)
        {
            btnEnter.onClick.AddListener(() =>
            {
                onOk?.Invoke(inputTextTMP.text);
            });
        }

        /// <summary>
        /// �ؽ�Ʈ ����
        /// </summary>
        /// <param name="promptText">�˾� �ȳ� ����</param>
        /// <param name="placeholderText">�Է� ���� �ȳ� �ؽ�Ʈ</param>
        /// <param name="basicInputText">�ؽ�Ʈ ���Է½� �⺻ �Է� �ؽ�Ʈ</param>
        public void SetText(string promptText, string placeholderText = "", string basicInputText = "")
        {
            promptTextTMP.text = promptText;
            placeholderTextTMP.text = placeholderText;
            inputTextTMP.text = basicInputText;
        }
    }

}
