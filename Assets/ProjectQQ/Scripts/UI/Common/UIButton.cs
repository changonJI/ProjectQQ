using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/UIButton")]
public class UIButton : Button
{
    [SerializeField] private UnityEvent onLongClick;

    private bool isPressed = false;
    private float pressTime = 0f;
    private float longClickTime = 1.0f;

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
    public override void OnPointerDown(PointerEventData eventData)
    {
        SetTime(0f);
        SetPress(true);
    }

    /// <summary>
    /// ��ư ���� ������� ���콺�� ������ ���� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public override void OnPointerUp(PointerEventData eventData)
    {
        SetPress(false);
    }

    /// <summary>
    /// ��ư ���� �ȿ��� ���콺�� ������ ���� �� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (pressTime >= longClickTime)
        {
            onLongClick?.Invoke();
        }
        else
        {
            onClick?.Invoke();
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

    /// <summary>
    /// <see cref="ButtonEditor"/>
    /// </summary>
    [CustomEditor(typeof(UIButton), true)]
    private class UIButtonEditor : SelectableEditor
    {
        private UIButton owner;
        SerializedProperty m_OnClickProperty;
        SerializedProperty m_OnClickLongProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");

            owner = (UIButton)target;
            m_OnClickLongProperty = serializedObject.FindProperty(nameof(owner.onLongClick));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            
            EditorGUILayout.PropertyField(m_OnClickProperty);

            // �߰�
            EditorGUILayout.PropertyField(m_OnClickLongProperty);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
