using UnityEditor;
using UnityEditor.UI;

namespace QQ
{
    /// <summary>
    /// <see cref="ButtonEditor"/>
    /// </summary>
    [CustomEditor(typeof(UIButton), true)]
    public class UIButtonEditor : SelectableEditor
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

            // Ãß°¡
            EditorGUILayout.PropertyField(m_OnClickLongProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}