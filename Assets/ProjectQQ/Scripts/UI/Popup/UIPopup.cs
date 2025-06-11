namespace QQ
{
    /// <summary>
    /// �˾� UI �⺻ Ŭ����
    /// </summary>
    public abstract class UIPopup<T> : UI<T> where T : UIPopup<T>
    {
        public override UIType uiType => UIType.Destroy;
        public override UIDepth uiDepth => UIDepth.Popup;

        protected bool isBackGroundClose = false;
    }
}

// NOTE
// * �˾� ����
//| �з�             | ����                                  | ��������                       |
//| ---------------- | ------------------------------------- | ------------------------------ |
//| ��ȣ�ۿ� �˾�    | ��ư���� ���� �Է� ����               | Alert, Confirm, Select, Prompt |
//| ���� ���� �˾�   | �ð� ������ �����, ��� �帧 �� ���� | Toast, Snackbar                |
//| Ȯ�� UI ������Ʈ | ȭ�� �����̼� �����̵�� �ߴ� UI      | Sheet, Drawer                  |