using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    // TODO
    // 1. �Ϻ� ���� Ư�� ���� ������ ���� �ν����Ϳ� �����ϵ��� �� ��� �ִ��� �˾ƺ�����
    // - isModal : backgroundBlocker ���� ���� ����
    // - isCloseOnBackgroundClick : ����� ��쿡�� ����
    // - dimColor : ���� �̹��� obj ���� ���� ����
    // - isUseCloseBtn : ��ư obj ������� ���� ����
    //
    // 2. ���� ��� ���� �ʿ�
    // ��Ÿ���� ��� - �����̵�, ���̵�, �׳� ��������..
    // ������� ��� - �����̵�, ���̵�, �׳� ��������..
    // ���� ȿ����
    // ���� ȿ����
    // ��ư ȿ����
    //
    // 3. �ߺ��ڵ� ����
    //


    /// <summary>
    /// �˾� UI �⺻ Ŭ����
    /// </summary>
    public abstract class UIPopup<T> : UI<T> where T : UIPopup<T>
    {
        public override UIType uiType => UIType.Back;
        public override UIDepth uiDepth => UIDepth.Popup;

        // �̰� ���� ��������....
        public static async UniTask<T> InstantiatePopup()
        {
            if (instance == null)
            {
                await ResManager.Instantiate(typeof(T));
            }

#if UNITY_EDITOR
            if (instance == null)
            {
                Debug.LogError($"{typeof(T)} type�� UI�� �߰��Ǿ� ���� �ʰų�, resource �� �������� ����");
                return null;
            }
#endif
            instance.SetActive(true);
            return instance;
        }

        /// <summary>
        /// ���� ���� ��ġ ���Ŀ (������ ��� active false)
        /// </summary>
        [SerializeField] protected GameObject backgroundBlocker;
        /// <summary>
        /// ����ΰ�? : �⺻ true
        /// </summary>
        [SerializeField] protected bool isModal;
        /// <summary>
        /// �ٱ� ���� Ŭ���� �ݱ� ��������?
        /// ������� �ƴ� ��� : false
        /// ������� ��� : �⺻ true
        /// </summary>
        [SerializeField] protected bool isCloseOnBackgroundClick;

        /// <summary>
        /// ���� ���� ���� �÷� ��� ���� �̹���
        /// </summary>
        [SerializeField] protected Image backgroundDimmer;
        /// <summary>
        /// ��� ���� �� �� backgroundDimmer�� ����
        /// </summary>
        [SerializeField] protected Color dimColor = new Color(0, 0, 0, 1f);

        /// <summary>
        /// �ݱ� ��ư
        /// </summary>
        [SerializeField] protected GameObject btnClose;
        /// <summary>
        /// �ݱ� ��ư ��� ����
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
// * �˾� ����
//| �з�             | ����                                  | ��������                       |
//| ---------------- | ------------------------------------- | ------------------------------ |
//| ��ȣ�ۿ� �˾�    | ��ư���� ���� �Է� ����               | Alert, Confirm, Select, Prompt |
//| ���� ���� �˾�   | �ð� ������ �����, ��� �帧 �� ���� | Toast, Snackbar                |
//| Ȯ�� UI ������Ʈ | ȭ�� �����̼� �����̵�� �ߴ� UI      | Sheet, Drawer                  |