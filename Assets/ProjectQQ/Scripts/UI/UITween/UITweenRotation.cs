using DG.Tweening;
using UnityEngine;


public class UITweenRotation : UITween
{
    [SerializeField] private bool isRight = true;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private bool isLoop = false;
    [SerializeField] private LoopType loopType = LoopType.Restart;

    protected override void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();

        Init(rect.DOLocalRotate(new Vector3(0, 0, isRight ? -90 : 90), duration)
            .SetEase(ease)
            .SetLoops(isLoop ? -1 : 0, loopType));
    }
}