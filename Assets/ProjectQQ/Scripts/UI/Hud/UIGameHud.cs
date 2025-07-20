using QQ;
using UnityEngine;

public class UIGameHud : UI<UIGameHud>
{
    public override UIType uiType => UIType.Main;
    public override UIDepth uiDepth => UIDepth.HUD;

    [SerializeField] private UIButtonAndText btnPause;
    
    private bool isPaused = false;

    protected override void OnInit()
    {
        btnPause.OnClickClear();
    }

    protected override void OnStart()
    {
        btnPause.OnClickAdd(OnClickPause);
    }

    protected override void OnFocus()
    {
        
    }

    protected override void OnLostFocus()
    {
        
    }

    protected override void OnExit()
    {
        btnPause.OnClickRemove(OnClickPause);
    }

    private void OnClickPause()
    {
        if (isPaused)
        {
            GameManager.Instance.GameResume();
            isPaused = false;
        }
        else
        {
            GameManager.Instance.GamePause();
            isPaused = true;
        }
    }
}
