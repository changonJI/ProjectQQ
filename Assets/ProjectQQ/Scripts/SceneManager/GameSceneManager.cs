using UnityEngine;

namespace QQ
{
    [DisallowMultipleComponent]
    public class GameSceneManager : MonoBehaviour
    {
        public Canvas canvas;

        private void Awake()
        {
        }

        async void Start()
        {
            // 배경 로드
            await ResManager.Instantiate(ResType.Stage, "Stage1");

            // 플레이어 로드
            await PoolManager.Instance.GetObject(GameObjectType.Actor, "Actor");

            // 몬스터 로드
        }

        void Update()
        {
        }
    }
}