namespace QQ
{
    /// <summary>
    /// 팝업 UI 기본 클래스
    /// </summary>
    public abstract class UIPopup<T> : UI<T> where T : UIPopup<T>
    {
        public override UIType uiType => UIType.Destroy;
        public override UIDepth uiDepth => UIDepth.Popup;

        protected bool isBackGroundClose = false;
    }
}

// NOTE
// * 팝업 종류
//| 분류             | 설명                                  | 세부종류                       |
//| ---------------- | ------------------------------------- | ------------------------------ |
//| 상호작용 팝업    | 버튼으로 유저 입력 받음               | Alert, Confirm, Select, Prompt |
//| 정보 전달 팝업   | 시간 지나면 사라짐, 사용 흐름 안 끊음 | Toast, Snackbar                |
//| 확장 UI 컴포넌트 | 화면 모퉁이서 슬라이드로 뜨는 UI      | Sheet, Drawer                  |