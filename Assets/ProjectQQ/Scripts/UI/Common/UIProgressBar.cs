using QQ;
using UnityEngine;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField] private UIImage ground;
    [SerializeField] private float minValue = 0f;
    [SerializeField] private float maxValue = 1f;
    [SerializeField] private float curValue = 0f;
    [Header("Reverses when the type is not filled")]
    [SerializeField] private bool isReverse = false;

    private Vector2 anchorMin = Vector2.zero;
    private Vector2 anchorMax = Vector2.one;

    private float nomalizedCurValue => Mathf.InverseLerp(minValue, maxValue, curValue);
    public float CurValue
    {
        get 
        {
            return curValue; 
        }

        set 
        { 
            curValue = curValue = Mathf.Clamp(value, minValue, maxValue);
            UpdateProgress();
        }
    }

    public void Init(float min = 0f, float max = 1f)
    {
        minValue = min;
        maxValue = max;
    }

    public void SetGroundColor(Color color)
    {
        ground.color = color;
    }

    public void UpdateProgress()
    {
        if (ground == null) return;

        if(ground.type == UnityEngine.UI.Image.Type.Filled)
        {
            ground.fillAmount = nomalizedCurValue;
        }
        else
        {
            if(isReverse)
                anchorMin[0] = 1f - nomalizedCurValue;
            else
                anchorMax[0] = nomalizedCurValue;
        }

        ground.rectTransform.anchorMin = anchorMin;
        ground.rectTransform.anchorMax = anchorMax;

        ground.rectTransform.offsetMin = Vector2.zero;
        ground.rectTransform.offsetMax = Vector2.zero;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (ground == null)
        {
            ground = new GameObject("ground").AddComponent<UIImage>();
            ground.transform.SetParent(transform);
        }
        
        UpdateProgress();
    }
#endif
}
