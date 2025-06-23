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
            // 첫 번째 플레이인지 구분
            if (GameManager.Instance.GetIntPlayerData(PlayerDataType.FirstPlay) == 0)
            {
                // 인트로 영상, 다이얼로그 출력
                UIDialogue.Instantiate(okAction: () =>
                {
                    // 다이얼로그 다 넘기면 저장
                    GameManager.Instance.SavePlayerData(PlayerDataType.FirstPlay, null, 1);
                    UIDialogue.CloseUI();
                    GameManager.Instance.GameStart(1, 1);
                    GameManager.Instance.GamePause(true);
                });
            }

            GameManager.Instance.LoadStage(1, 1);

            // 플레이어 로드
            ResManager.Instantiate(ResType.Object, "Actor").Forget();


        }

        void Update()
        {

        }
    }
}