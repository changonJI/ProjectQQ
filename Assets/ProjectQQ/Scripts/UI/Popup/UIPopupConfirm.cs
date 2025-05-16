using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;


namespace QQ
{
    /// <summary>
    /// 확인/취소 팝업
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
        /// 확인 팝업 버튼 동작 세팅
        /// </summary>
        /// <param name="onOk">확인 버튼 OnClick 동작</param>
        /// <param name="onCancel">취소 버튼 OnClick 동작</param>
        /// <param name="tcs">확인 취소 버튼 클릭 결과 받아갈 변수</param>
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
        /// 텍스트 세팅
        /// </summary>
        /// <param name="messageText">팝업 안내 문구</param>
        public void SetText(string messageText)
        {
            messageTextTMP.text = messageText;
        }
    }
}
