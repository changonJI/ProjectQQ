using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private CameraManager cameraManager;
        GameObject stage;
        private void Awake()
        {
            cameraManager.Init();
        }

        private void Start()
        {
            Init().Forget();
        }

        void Update()
        {
        }

        private async UniTaskVoid Init()
        {            
            // 스테이지 로드  // TODO. stage BaseGameObject 통해서 값 넣도록 변경
            stage = await ResManager.Instantiate(ResType.Stage, "Stage1");

            cameraManager.SetCameraTarget(CameraType.Default, stage.transform);

            await UniTask.WaitForSeconds(3f); // 맵 로드 후 딜레이
            // 플레이어 로드
            GameObject actor = await PoolManager.Instance.GetObject(GameObjectType.Actor, "Actor", 1);
            cameraManager.SetCameraTarget(CameraType.Player, actor.transform);

            // 몬스터 로드
            MonsterSpawner spawner = stage.GetComponent<MonsterSpawner>();
            if (null != spawner)
            {
                float cameraHalfH = cameraManager.GetCameraOrthographicSize(CameraType.Player);
                float cameraHalfW = cameraHalfH * cameraManager.GetCameraAspect();

                spawner.SetStage(10, 1, stage.GetComponent<GridManager>(), cameraHalfW, cameraHalfH);
            }
        }
    }
}