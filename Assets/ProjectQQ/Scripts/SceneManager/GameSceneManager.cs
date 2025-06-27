using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public class GameSceneManager : MonoBehaviour
    {        
        private void Awake()
        {
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
            await GameManager.Instance.InitCamera();

            // 배경 로드
            await ResManager.Instantiate(ResType.Stage, "Stage1");

            // 플레이어 로드
            GameObject actor = await PoolManager.Instance.GetObject(GameObjectType.Actor, "Actor");
            GameManager.Instance.SetCameraTarget(actor.transform);

            // 몬스터 로드
        }
    }
}