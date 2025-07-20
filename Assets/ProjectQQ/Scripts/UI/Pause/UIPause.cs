using QQ;
using UnityEngine;

public class UIPause : UI<UIPause>
{
    public override UIType uiType => UIType.Destroy;
    public override UIDepth uiDepth => UIDepth.Fixed1;

    [SerializeField] private UIButtonAndText btnSettings;
    [SerializeField] private UIButtonAndText btnExitGame;
    
    protected override void OnInit()
    {
        btnSettings.OnClickClear();
        btnExitGame.OnClickClear();
    }

    protected override void OnStart()
    {
        btnSettings.OnClickAdd(OnClickSettings);
        btnExitGame.OnClickAdd(OnClickBackToMain);
    }

    protected override void OnFocus()
    {
        
    }

    protected override void OnLostFocus()
    {
        
    }

    protected override void OnExit()
    {
        btnSettings.OnClickRemove(OnClickSettings);
        btnExitGame.OnClickRemove(OnClickBackToMain);
    }

    private void OnClickBackToMain()
    {
        // 진짜 게임 나가겠습니까? 팝업
        CloseUI();
        GameManager.Instance.BackToMain();
    }

    private void OnClickSettings()
    {
        // UISettings.Instantiate();
        Debug.Log("세팅 팝업 생성");
    }
}
