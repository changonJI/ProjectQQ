using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace QQ
{
    // TODO
    // ID 받아서 LangTable 텍스트 가져오는 기능 추가 필요
    // 중복코드 통합정리 필요

    /// <summary>
    /// 팝업 UI 열기 관장
    /// Open~ 함수로 팝업창 호출,
    /// Open함수 내부에서 Open~Async 함수로 할당 후 값 세팅
    /// </summary>
    public static class UIPopupManager
    {
        /// <summary>
        /// 확인 팝업 열기
        /// </summary>
        /// <param name="onOk"></param>
        /// <param name="onCancel"></param>
        /// <returns>확인 팝업 버튼 선택 결과</returns>
        public static async UniTask<bool> OpenConfirmAsync(Action onOk = null, Action onCancel = null)
        {
            var tcs = new UniTaskCompletionSource<bool>();

            UIPopupConfirm popup = await UIPopupConfirm.InstantiatePopup();
            popup.SetBtnAction(onOk, onCancel, tcs);

            if (null != popup)
            {
                popup.SetText("이름 추후 변경 불가");
            }
            else
            {
            }

            return await tcs.Task;
        }

        /// <summary>
        /// 텍스트 입력 팝업 열기
        /// </summary>
        /// <param name="onOK">Enter 동작</param>
        /// <param name="isUseConfirm">입력값 확인 팝업 사용 여부</param>
        public static void OpenPrompt(Action<string> onOK = null, bool isUseConfirm = false)
        {
            OpenPromptAsync(onOK, isUseConfirm).Forget();
        }

        /// <summary>
        /// OpenPrompt 실행 메소드
        /// </summary>
        /// <param name="onOK">Enter 동작</param>
        /// <param name="isUseConfirm">입력값 확인 팝업 사용 여부</param>
        private static async UniTaskVoid OpenPromptAsync(Action<string> onOK, bool isUseConfirm)
        {
            UIPopupPrompt popup = await UIPopupPrompt.InstantiatePopup();
            if (null == popup)
            {
                return;
            }

            popup.SetText("요원명을 입력하셈 ^0^~", "여기에 쓰기");

            popup.SetBtnAction(async inputName =>
            {
                if (true == isUseConfirm)
                {
                    bool result = await UIPopupManager.OpenConfirmAsync();
                    if (true == result)
                    {
                        onOK?.Invoke(inputName); // 확인 OK일 때만
                        popup.Close();
                    }
                    else
                    {
                        // 취소하면 아무 일 안 일어남
                    }
                }
                else
                {
                    onOK?.Invoke(inputName);
                    popup.Close();
                }
            });
        }
    }
}