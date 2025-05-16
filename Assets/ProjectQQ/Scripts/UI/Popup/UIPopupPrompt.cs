using System;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace QQ
{
    /// <summary>
    /// 텍스트 입력 팝업
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
        /// 텍스트 입력 팝업 버튼 동작 세팅
        /// </summary>
        /// <param name="onOk">확인 버튼 OnClick 동작</param>
        public void SetBtnAction(Action<string> onOk)
        {
            btnEnter.onClick.AddListener(() =>
            {
                onOk?.Invoke(inputTextTMP.text);
            });
        }

        /// <summary>
        /// 텍스트 세팅
        /// </summary>
        /// <param name="promptText">팝업 안내 문구</param>
        /// <param name="placeholderText">입력 영역 안내 텍스트</param>
        /// <param name="basicInputText">텍스트 미입력시 기본 입력 텍스트</param>
        public void SetText(string promptText, string placeholderText = "", string basicInputText = "")
        {
            promptTextTMP.text = promptText;
            placeholderTextTMP.text = placeholderText;
            inputTextTMP.text = basicInputText;
        }
    }

}
