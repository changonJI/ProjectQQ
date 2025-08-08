using Cysharp.Threading.Tasks;
using ProjectQQ.Scripts.UI.Popup;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private CameraManager cameraManager;
        Stage stage;
        private void Awake()
        {
            cameraManager.Init();
        }

        private void Start()
        {
            Init().Forget();
            UIGameHud.Instantiate();
        }

        private async UniTaskVoid Init()
        {            
            // 스테이지 로드  // TODO. stage BaseGameObject 통해서 값 넣도록 변경
            GameObject stageObject = await ResManager.AsyncInstantiate(ResType.Stage, "Stage1");
            stage = stageObject.GetComponent<Stage>();

            cameraManager.SetCameraTarget(CameraType.Default, stage.transform);

            await UniTask.WaitForSeconds(3f); // 맵 로드 후 딜레이
            // 플레이어 로드
            GameObject actor = await PoolManager.Instance.GetObject(GameObjectType.Actor, "Actor", Vector3.zero, PlayerStatDataManager.Instance.Get(1).playerLevel);
            cameraManager.SetCameraTarget(CameraType.Player, actor.transform);

            // 몬스터 스포너 세팅
            float cameraHalfH = cameraManager.GetCameraOrthographicSize(CameraType.Player);
            float cameraHalfW = cameraHalfH * cameraManager.GetCameraAspect();
            stage.SetMonsterSpawner(cameraHalfW, cameraHalfH);
        }
        
        [ContextMenu("스테이지 클리어")]
        private void PlayerLevelUp()
        {
            GameManager.Instance.TimeScaleChanger(true); // 게임 일시 정지
            UIClearReward.Instantiate(); // 룰렛 UI 호출
        }

        private void StageClearFail()
        {
            GameManager.Instance.TimeScaleChanger(true); // 게임 일시 정지
            // 실패 UI.Instantiate;
        }
    }
}