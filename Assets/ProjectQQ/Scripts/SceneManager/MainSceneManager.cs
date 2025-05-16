using System;
using UnityEngine;
using UnityEngine.Rendering;

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
            UIMainScene.Instantiate();

            // 플레이어 데이터 확인, 없으면 이름 입력 팝업 띄우기 요청
            if (true)
            {   // 저장된 데이터 없는 경우
                UIPopupManager.OpenPrompt(inputName =>
                {
                    // 이름 저장 로직 추가 필요
#if UNITY_EDITOR
                    Debug.Log($"이름 '{inputName}' 입력함");
#endif
                }, true);
            }
        }
    }
}