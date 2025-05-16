using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UITweenColor : MonoBehaviour
{
    private Image image;
    [SerializeField] private Color startColor;
    [SerializeField] private Color EndColor;
    [SerializeField] private float duration = 1f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private bool isLoop = false;
    [SerializeField] private LoopType loopType = LoopType.Restart;

    private void Start()
    {
        image = GetComponent<Image>();

        if (image == null)
            return;

        image.color = startColor;

        image.DOColor(EndColor, duration)
            .SetEase(ease)
            .SetLoops(isLoop ? -1 : 0, loopType);
    }
}
