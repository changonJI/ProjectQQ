using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    public sealed class UIDialogue : UI<UIDialogue>
    {
        public override UIType uiType => UIType.Destroy;
        public override UIDepth uiDepth => UIDepth.Fixed3;

        [Header("Top")]
        [SerializeField] private UIButtonAndText btnSkip;
        [Header("Center")]
        [SerializeField] private UIImage imgBg;
        [SerializeField] private RawImage imgL;
        [SerializeField] private RawImage imgC;
        [SerializeField] private RawImage imgR;
        [Header("Bottom")]
        [SerializeField] private TextMeshProUGUI txtTalk;
        [SerializeField] private UIButtonAndText btnNext;

        private int chapterIndex = 0;
        private int page = 0;
        //private var tableData;

        private System.Action OnNextScene;

        protected override void OnInit()
        {
            // tableData = null;
            imgBg.sprite = null;
            imgL.texture = null;
            imgC.texture = null;
            imgR.texture = null;

            txtTalk.text = string.Empty;

            chapterIndex = 0;
            page = 0;

            btnNext.OnClickClear();
            btnSkip.OnClickClear();

            OnNextScene = null;
        }

        protected override void OnStart()
        {
            btnNext.OnClickAdd(OnClickNext);
            btnNext.SetText("Test");
            btnSkip.OnClickAdd(OnClickSkip);
            btnSkip.SetText("Test");

            chapterIndex = GetParamToInt(0);

            txtTalk.text = string.Empty;

            OnNextScene = OnClickOk;

            InitUI();
        }

        protected override void OnFocus()
        {
            
        }

        protected override void OnExit()
        {
            btnNext.OnClickRemove(OnClickNext);
            btnNext.SetText(string.Empty);
            btnSkip.OnClickRemove(OnClickSkip);
            btnSkip.SetText(string.Empty);

            chapterIndex = 0;

            OnNextScene = null;
        }

        protected override void OnLostFocus()
        {
        }

        /// <summary>
        /// Dialogue UI 초기화
        /// </summary>
        private void InitUI()
        {
            //TODO: 저장된 데이터에서 현재 챕터를 깼는지 안깼는지에따라 skip 버튼 on/off 처리하기
            //btnSkip.gameObject.SetActive(false); // TODO: Set Skip Button Active State

            //TODO: Table Data ChapterIndex로 Get하기
            //tableData = DialogueDataMagaer.Instance.Get(chapterIndex);
            //imgBg.sprite.; // TODO: Set Background Sprite
            imgL.texture = null; // TODO: Set Left Image Sprite
            imgC.texture = null; // TODO: Set Center Image Sprite
            imgR.texture = null; // TODO: Set Right Image Sprite
            SetImage(UIDialouguePos.None, "");
        }

        private void OnClickNext()
        {
            //TODO : 마지막 페이지 체크
            if (page == 1)
            {
                OnNextScene?.Invoke();
            }
            else
            {
                Debug.Log("TestNest");
                SetImage(UIDialouguePos.Left, "Popup");

                page++;
            }
        }

        private void OnClickSkip()
        {

        }

        private void SetImage(UIDialouguePos pos, string fileName)
        {
            switch (pos)
            {
                case UIDialouguePos.None:
                    imgL.color = Color.clear;
                    imgR.color = Color.clear;
                    imgC.color = Color.clear;
                    break;
                case UIDialouguePos.Left:
                    imgL.texture = ResManager.LoadResource<Texture>(ResType.Texture, fileName);
                    imgL.color = Color.white;
                    imgC.color = Color.clear;
                    imgR.color = Color.clear;
                    break;
                case UIDialouguePos.Right:
                    imgR.texture = ResManager.LoadResource<Texture>(ResType.Texture, fileName);
                    imgR.color = Color.white;
                    imgL.color = Color.clear;
                    imgC.color = Color.clear;
                    break;
                case UIDialouguePos.Center:
                    imgC.texture = ResManager.LoadResource<Texture>(ResType.Texture, fileName);
                    imgC.color = Color.white;
                    imgL.color = Color.clear;
                    imgR.color = Color.clear;
                    break;
                default:
                    LogHelper.LogError("UIDialogue SetImage Error: Invalid position specified.");
                    break;
            }
        }
    }
}
