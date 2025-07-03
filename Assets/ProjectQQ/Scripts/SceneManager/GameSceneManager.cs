using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private CameraManager cameraManager;

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
            // 배경 로드
            GameObject map = await ResManager.Instantiate(ResType.Stage, "Stage1");
            cameraManager.SetCameraTarget(CameraType.Default, map.transform);

            await UniTask.WaitForSeconds(3f); // 맵 로드 후 딜레이
            // 플레이어 로드
            GameObject actor = await PoolManager.Instance.GetObject(GameObjectType.Actor, "Actor");
            cameraManager.SetCameraTarget(CameraType.Player, actor.transform);

            // 몬스터 로드
        }
    }
}