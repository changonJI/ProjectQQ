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
            // InputManager 추가 필요
            // SoundManager 추가 필요
            // EffectManager 추가 필요
            // PoolManager 추가 필요
            UIMainScene.Instantiate();
        }
        
    }
}