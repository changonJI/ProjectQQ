using ProjectQQ;
using System.Collections;
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

        private IEnumerator Start()
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
                VideoManager.Instance.Init();
            }

            if (!isFirst)
                isFirst = true;

            if (!GameManager.Instance.IsValidPlayer())
            {
                yield return new WaitForSeconds(1f);

                VideoManager.Instance.PlayViedo("QQ_intro");
            }

            UIMainScene.Instantiate();
        }
    }
}