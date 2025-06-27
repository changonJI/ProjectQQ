using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    // TODO
    // 이름 실제 저장 코드 작성 필요
    // 플레이어 데이터 체크 코드 필요
    public sealed class MainSceneManager : MonoBehaviour
    {
        private void Awake()
        {
            LanguageDataManager.Instance.LoadData();
        }

        private void Start()
        {
            Init().Forget();
        }
        
        private async UniTaskVoid Init()
        {
            //Manager들 추가
            GameManager.Instance.Init();
            InputManager.Instance.Init();
            SoundManager.Instance.Init();
            EffectManager.Instance.Init();
            PoolManager.Instance.Init();

            await GameManager.Instance.InitCamera();

            UIMainScene.Instantiate();
        }
    }
}