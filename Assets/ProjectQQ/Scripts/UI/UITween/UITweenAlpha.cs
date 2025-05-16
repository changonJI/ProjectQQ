using DG.Tweening;
using UnityEngine;

public class UITweenAlpha : MonoBehaviour
{
    private CanvasGroup group;
    [Range(0f, 1f)][SerializeField] private float alpha;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private bool isLoop = false;
    [SerializeField] private LoopType loopType = LoopType.Restart;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();

        if (group == null)
            return;

        group.alpha = 0f;

        group.DOFade(alpha, duration).
            SetEase(ease);
    }
}
