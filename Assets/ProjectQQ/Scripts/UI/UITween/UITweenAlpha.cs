using DG.Tweening;
using UnityEngine;

public class UITweenAlpha : UITween
{
    private CanvasGroup group;
    [Range(0f, 1f)][SerializeField] private float endAlpha;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private bool isLoop = false;
    [SerializeField] private LoopType loopType = LoopType.Restart;

    protected override void Awake()
    {
        group = GetComponent<CanvasGroup>();

        if (group == null)
            return;

        group.alpha = 0f;

        Init(group.DOFade(endAlpha, duration)
            .SetEase(ease)
            .SetLoops(isLoop ? -1 : 0, loopType));
    }
}
