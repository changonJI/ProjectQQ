using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;


namespace QQ
{
    /// <summary>
    /// Ȯ��/��� �˾�
    /// </summary>
    public sealed class UIPopupConfirm : UIPopup<UIPopupConfirm>
    {
        [SerializeField] private TextMeshProUGUI messageTextTMP;
        [SerializeField] private UIButton okButton;
        [SerializeField] private UIButton cancelButton;

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
        /// Ȯ�� �˾� ��ư ���� ����
        /// </summary>
        /// <param name="onOk">Ȯ�� ��ư OnClick ����</param>
        /// <param name="onCancel">��� ��ư OnClick ����</param>
        /// <param name="tcs">Ȯ�� ��� ��ư Ŭ�� ��� �޾ư� ����</param>
        public void SetBtnAction(Action onOk = null, Action onCancel = null, UniTaskCompletionSource<bool> tcs = null)
        {
            okButton.onClick.AddListener(() =>
            {
                onOk?.Invoke();
                tcs?.TrySetResult(true);

                Close();
            });

            cancelButton.onClick.AddListener(() =>
            {
                onCancel?.Invoke();
                tcs?.TrySetResult(false);

                Close();
            });
        }

        /// <summary>
        /// �ؽ�Ʈ ����
        /// </summary>
        /// <param name="messageText">�˾� �ȳ� ����</param>
        public void SetText(string messageText)
        {
            messageTextTMP.text = messageText;
        }
    }
}
