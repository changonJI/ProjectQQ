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

    public void OnPointerUp(PointerEventData eventData)
    {
        if(pressTime >= longClickTime)
        {
            onLongClickEvent?.Invoke();
        }
        else
        {
            button.onClick.Invoke();
        }

        isPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = true;
    }
}
