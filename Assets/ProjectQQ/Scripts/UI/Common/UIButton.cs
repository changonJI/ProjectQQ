using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private Button button;
    
    [SerializeField] private UnityEvent onLongClickEvent;
    private bool isPressed = false;
    private float pressTime = 0f;
    private float longClickTime = 1.0f;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (isPressed)
        {
            pressTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// 버튼 범위 안에서 마우스를 누르는 순간의 이벤트
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        SetTime(0f);
        SetPress(true);
    }

    /// <summary>
    /// 버튼 범위 상관없이 마우스를 누르고 땠을 때 발생하는 이벤트
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        SetPress(false);
    }

    /// <summary>
    /// 버튼 범위 안에서 마우스를 누르고 땟을 때 발생하는 이벤트
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pressTime >= longClickTime)
        {
            onLongClickEvent?.Invoke();
        }
        else
        {
            button.onClick.Invoke();
        }

        SetPress(false);
    }

    private void SetPress(bool press)
    {
        isPressed = press;
        
    }
    private void SetTime(float time)
    {
        pressTime = time;
    }
}
