using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    // TODO
    // 1. 일부 변수 특정 조건 만족할 때만 인스펙터에 노출하도록 할 방법 있는지 알아봐야함
    // - isModal : backgroundBlocker 있을 때만 노출
    // - isCloseOnBackgroundClick : 모달인 경우에만 노출
    // - dimColor : 바탕 이미지 obj 있을 때만 노출
    // - isUseCloseBtn : 버튼 obj 등록했을 때만 노출
    //
    // 2. 연출 기능 구현 필요
    // 나타나는 방식 - 슬라이드, 페이드, 그냥 없어지기..
    // 사라지는 방식 - 슬라이드, 페이드, 그냥 없어지기..
    // 열림 효과음
    // 닫힘 효과음
    // 버튼 효과음
    //
    // 3. 중복코드 정리
    //


    /// <summary>
    /// 팝업 UI 기본 클래스
    /// </summary>
    public abstract class UIPopup<T> : UI<T> where T : UIPopup<T>
    {
        public override UIType uiType => UIType.Back;
        public override UIDepth uiDepth => UIDepth.Popup;

        // 이걸 으케 정리하지....
        public static async UniTask<T> InstantiatePopup()
        {
            if (instance == null)
            {
                await ResManager.Instantiate(typeof(T));
            }

#if UNITY_EDITOR
            if (instance == null)
            {
                Debug.LogError($"{typeof(T)} type이 UI에 추가되어 있지 않거나, resource 가 존재하지 않음");
                return null;
            }
#endif
            instance.SetActive(true);
            return instance;
        }

        /// <summary>
        /// 바탕 영역 터치 블로커 (논모달인 경우 active false)
        /// </summary>
        [SerializeField] protected GameObject backgroundBlocker;
        /// <summary>
        /// 모달인가? : 기본 true
        /// </summary>
        [SerializeField] protected bool isModal;
        /// <summary>
        /// 바깥 영역 클릭시 닫기 동작할지?
        /// ·모달이 아닌 경우 : false
        /// ·모달인 경우 : 기본 true
        /// </summary>
        [SerializeField] protected bool isCloseOnBackgroundClick;

        /// <summary>
        /// 바탕 영역 딤드 컬러 깔기 위한 이미지
        /// </summary>
        [SerializeField] protected Image backgroundDimmer;
        /// <summary>
        /// 배경 딤드 색 ☞ backgroundDimmer에 적용
        /// </summary>
        [SerializeField] protected Color dimColor = new Color(0, 0, 0, 1f);

        /// <summary>
        /// 닫기 버튼
        /// </summary>
        [SerializeField] protected GameObject btnClose;
        /// <summary>
        /// 닫기 버튼 사용 여부
        /// </summary>
        [SerializeField] protected bool isUseCloseBtn;


        protected override void OnInit()
        {
            if (default != backgroundBlocker)
            {
                backgroundBlocker.SetActive(isModal);
            }

            if (default != backgroundDimmer)
            {
                backgroundDimmer.color = dimColor;
            }

            if (default != btnClose)
            {
                btnClose.SetActive(isUseCloseBtn);
            }
        }

        public override void Close()
        {
            base.Close();
        }

        public virtual void Setting(bool isModal, bool isUseCloseBtn, Color dimColor)
        {
            this.isModal = isModal;
            this.isUseCloseBtn = isUseCloseBtn;
            this.dimColor = dimColor;
        }
    }
}

// NOTE
// * 팝업 종류
//| 분류             | 설명                                  | 세부종류                       |
//| ---------------- | ------------------------------------- | ------------------------------ |
//| 상호작용 팝업    | 버튼으로 유저 입력 받음               | Alert, Confirm, Select, Prompt |
//| 정보 전달 팝업   | 시간 지나면 사라짐, 사용 흐름 안 끊음 | Toast, Snackbar                |
//| 확장 UI 컴포넌트 | 화면 모퉁이서 슬라이드로 뜨는 UI      | Sheet, Drawer                  |