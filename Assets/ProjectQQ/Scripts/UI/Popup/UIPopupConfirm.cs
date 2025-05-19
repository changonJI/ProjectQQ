using TMPro;
using UnityEngine;


namespace QQ
{
    /// <summary>
    /// Ȯ��/��� �˾�
    /// </summary>
    public sealed class UIPopupConfirm : UIPopup<UIPopupConfirm>
    {
        [SerializeField] private TextMeshProUGUI messageTextTMP;
        [SerializeField] private UIButtonAndText btnOk;
        [SerializeField] private UIButtonAndText btnCancel;
        
        protected override void OnInit()
        {
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnFocus()
        {
            SetTitle(GetParamToString(0));

            btnOk.SetText(GetParamToString(1));
            btnCancel.SetText(GetParamToString(2));

            btnOk.OnClickAdd(OnClickOk);
            btnCancel.OnClickAdd(OnClickClose);
        }

        protected override void OnLostFocus()
        {
            btnOk.OnClickRemove(OnClickOk);
            btnCancel.OnClickRemove(OnClickClose);
        }

        /// <summary>
        /// �˾� ���� ����
        /// </summary>
        /// <param name="messageText"></param>
        private void SetTitle(string messageText)
        {
            messageTextTMP.text = messageText;
        }
    }
}
