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
            // 배경 로드
            ResManager.Instantiate(ResType.Stage, "Stage1");

            // 플레이어 로드
            ResManager.Instantiate(ResType.Object, "Actor");

            // 몬스터 로드
        }

        void Update()
        {
        }
    }
}