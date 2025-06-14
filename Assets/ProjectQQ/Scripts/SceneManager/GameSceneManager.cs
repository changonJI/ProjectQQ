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
            // ��� �ε�
            ResManager.Instantiate(ResType.Stage, "Stage1");

            // �÷��̾� �ε�
            ResManager.Instantiate(ResType.Object, "Actor");

            // ���� �ε�
        }

        void Update()
        {
        }
    }
}