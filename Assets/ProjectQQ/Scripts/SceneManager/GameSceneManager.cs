using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace QQ
{
    [DisallowMultipleComponent]
    public class GameSceneManager : MonoBehaviour
    {
        public Canvas canvas;

        private void Awake()
        {
        }

        void Start()
        {
            // ù ��° �÷������� ����
            if (GameManager.Instance.GetIntPlayerData(PlayerDataType.FirstPlay) == 0)
            {
                // ��Ʈ�� ����, ���̾�α� ���
                UIDialogue.Instantiate(okAction: () =>
                {
                    // ���̾�α� �� �ѱ�� ����
                    GameManager.Instance.SavePlayerData(PlayerDataType.FirstPlay, null, 1);
                    UIDialogue.CloseUI();
                    GameManager.Instance.GameStart(1, 1);
                    GameManager.Instance.GamePause(true);
                });
            }

            GameManager.Instance.LoadStage(1, 1);

            // �÷��̾� �ε�
            ResManager.Instantiate(ResType.Object, "Actor").Forget();


        }

        void Update()
        {

        }
    }
}