using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace QQ
{
    // TODO
    // ID �޾Ƽ� LangTable �ؽ�Ʈ �������� ��� �߰� �ʿ�
    // �ߺ��ڵ� �������� �ʿ�

    /// <summary>
    /// �˾� UI ���� ����
    /// Open~ �Լ��� �˾�â ȣ��,
    /// Open�Լ� ���ο��� Open~Async �Լ��� �Ҵ� �� �� ����
    /// </summary>
    public static class UIPopupManager
    {
        /// <summary>
        /// Ȯ�� �˾� ����
        /// </summary>
        /// <param name="onOk"></param>
        /// <param name="onCancel"></param>
        /// <returns>Ȯ�� �˾� ��ư ���� ���</returns>
        public static async UniTask<bool> OpenConfirmAsync(Action onOk = null, Action onCancel = null)
        {
            var tcs = new UniTaskCompletionSource<bool>();

            UIPopupConfirm popup = await UIPopupConfirm.InstantiatePopup();
            popup.SetBtnAction(onOk, onCancel, tcs);

            if (null != popup)
            {
                popup.SetText("�̸� ���� ���� �Ұ�");
            }
            else
            {
            }

            return await tcs.Task;
        }

        /// <summary>
        /// �ؽ�Ʈ �Է� �˾� ����
        /// </summary>
        /// <param name="onOK">Enter ����</param>
        /// <param name="isUseConfirm">�Է°� Ȯ�� �˾� ��� ����</param>
        public static void OpenPrompt(Action<string> onOK = null, bool isUseConfirm = false)
        {
            OpenPromptAsync(onOK, isUseConfirm).Forget();
        }

        /// <summary>
        /// OpenPrompt ���� �޼ҵ�
        /// </summary>
        /// <param name="onOK">Enter ����</param>
        /// <param name="isUseConfirm">�Է°� Ȯ�� �˾� ��� ����</param>
        private static async UniTaskVoid OpenPromptAsync(Action<string> onOK, bool isUseConfirm)
        {
            UIPopupPrompt popup = await UIPopupPrompt.InstantiatePopup();
            if (null == popup)
            {
                return;
            }

            popup.SetText("������� �Է��ϼ� ^0^~", "���⿡ ����");

            popup.SetBtnAction(async inputName =>
            {
                if (true == isUseConfirm)
                {
                    bool result = await UIPopupManager.OpenConfirmAsync();
                    if (true == result)
                    {
                        onOK?.Invoke(inputName); // Ȯ�� OK�� ����
                        popup.Close();
                    }
                    else
                    {
                        // ����ϸ� �ƹ� �� �� �Ͼ
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