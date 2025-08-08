using UnityEngine;

namespace QQ
{
    // TODO
    // 이름 실제 저장 코드 작성 필요
    // 플레이어 데이터 체크 코드 필요
    public sealed class MainSceneManager : MonoBehaviour
    {
        // 첫 진입시 체크(UI에서 데이터 세팅 용도로 start 버튼같은거 필요할듯)
        public static bool isFirst = false;

        private void Awake()
        {
            if(!isFirst)
                TableDataManager.LoadTableData();
        }

        private void Start()
        {
            Init();
        } 
        
        private void Init()
        {
            if (!isFirst)
            {
                //UIRoot Canvas 추가
                ResManager.Instantiate(ResType.UI, "UIRoot");

                //Manager들 추가
                GameManager.Instance.Init();
                InputManager.Instance.Init();
                SoundManager.Instance.Init();
                SkillManager.Instance.Init();
                PoolManager.Instance.Init();

                //TODO: VideoManger 추가 필요
            }

            if (!isFirst)
                isFirst = true;

            //TODO : Intro 체크
            if (GameManager.Instance.IsValidPlayer())
            {
                //TODO: VideoManager 실행
            }

            UIMainScene.Instantiate();
        }
    }
}