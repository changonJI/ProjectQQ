using UnityEngine;

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
            // InputManager �߰� �ʿ�
            // SoundManager �߰� �ʿ�
            // EffectManager �߰� �ʿ�
            // PoolManager �߰� �ʿ�

            if (GameManager.Instance.GetStringPlayerData(PlayerDataType.UserName) == string.Empty)
            {
                UICreateNickName.Instantiate();
            }
            else
            {
                UIMainScene.Instantiate();
            }
        }
        
    }
}