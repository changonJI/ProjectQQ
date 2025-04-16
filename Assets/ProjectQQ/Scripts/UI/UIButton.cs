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
    /// ��ư ���� �ȿ��� ���콺�� ������ ������ �̺�Ʈ
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        SetTime(0f);
        SetPress(true);
    }

    /// <summary>
    /// ��ư ���� ������� ���콺�� ������ ���� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        SetPress(false);
    }

    /// <summary>
    /// ��ư ���� �ȿ��� ���콺�� ������ ���� �� �߻��ϴ� �̺�Ʈ
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
