using DG.Tweening;
using QQ;
using UnityEngine;

public class UITween : MonoBehaviour
{
    protected Tween MyTween;

    protected virtual void Awake()
    {
        MyTween = null;
    }

    protected virtual void OnEnable()
    {
        if (MyTween == null) return;
        LogHelper.Log("Tween OnEable");
        MyTween.Restart();
    }

    protected virtual void OnDisable()
    {
        LogHelper.Log("Tween Dis");

        if (MyTween != null)
        {
            MyTween.Kill();
        }
    }

    protected virtual void OnDestroy()
    {
        if (MyTween != null)
        {
            MyTween.Kill();
            MyTween = null;
        }
    }

    protected void Init(Tween tween)
    {
        MyTween = tween;
        MyTween.SetAutoKill(false);
        MyTween.Pause();
    }
}