using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace QQ
{
    // TODO
    // �̸� ���� ���� �ڵ� �ۼ� �ʿ�
    // �÷��̾� ������ üũ �ڵ� �ʿ�
    public sealed class MainSceneManager : MonoBehaviour
    {
        private void Awake()
        {
            LanguageDataManager.Instance.LoadData();
        }

        private void Start()
        {
            UIMainScene.Instantiate();

            // �÷��̾� ������ Ȯ��, ������ �̸� �Է� �˾� ���� ��û
            if (true)
            {   // ����� ������ ���� ���
                UIPopupManager.OpenPrompt(inputName =>
                {
                    // �̸� ���� ���� �߰� �ʿ�
#if UNITY_EDITOR
                    Debug.Log($"�̸� '{inputName}' �Է���");
#endif
                }, true);
            }
        }
    }
}