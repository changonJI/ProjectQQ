using TMPro;
using UnityEngine;

namespace QQ
{
    public sealed class UIDialogue : UI<UIDialogue>
    {
        public override UIType uiType => UIType.Back;
        public override UIDepth uiDepth => UIDepth.Fixed3;

        [Header("Top")]
        [SerializeField] private UIButtonAndText btnSkip;
        [Header("Center")]
        [SerializeField] private UIImage imgBg;
        [SerializeField] private UIImage imgL;
        [SerializeField] private UIImage imgC;
        [SerializeField] private UIImage imgR;
        [Header("Bottom")]
        [SerializeField] private TextMeshProUGUI txtTalk;
        [SerializeField] private UIButtonAndText btnNext;

        private int chapterIndex = 0;
        //private var tableData;

        protected override void OnInit()
        {
            // tableData = null;
            imgBg.sprite = null;
            imgL.sprite = null;
            imgC.sprite = null;
            imgR.sprite = null;

            txtTalk.text = string.Empty;

            btnNext.OnClickClear();
            btnSkip.OnClickClear();
        }

        protected override void OnStart()
        {
        }

        protected override void OnFocus()
        {
            btnNext.OnClickAdd(OnClickOk);
            btnNext.SetText("Test");
            btnSkip.OnClickAdd(OnClickOk);
            btnSkip.SetText("Test");

            chapterIndex = GetParamToInt(0);

            txtTalk.text = string.Empty;

            InitUI();
        }

        protected override void OnLostFocus()
        {
            btnNext.OnClickRemove(OnClickOk);
            btnNext.SetText(string.Empty);
            btnSkip.OnClickRemove(OnClickOk);
            btnSkip.SetText(string.Empty);

            chapterIndex = 0;
        }

        /// <summary>
        /// Dialogue UI �ʱ�ȭ
        /// </summary>
        private void InitUI()
        {
            //TODO: ����� �����Ϳ��� ���� é�͸� ������ �Ȳ����������� skip ��ư on/off ó���ϱ�
            //btnSkip.gameObject.SetActive(false); // TODO: Set Skip Button Active State

            //TODO: Table Data ChapterIndex�� Get�ϱ�
            //tableData = DialogueDataMagaer.Instance.Get(chapterIndex);
            //imgBg.sprite = ResManager.Load(tableData.LeftImage); // TODO: Set Background Sprite
            //imgL.sprite = null; // TODO: Set Left Image Sprite
            //imgC.sprite = null; // TODO: Set Center Image Sprite
            //imgR.sprite = null; // TODO: Set Right Image Sprite
        }

        private void OnClickNext()
        {
            
        }

        private void OnClickSkip()
        {

        }
    }
}
