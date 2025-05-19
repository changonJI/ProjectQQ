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
        //NOTE: Inspector창 등록이 아닌 코드 등록 방식 사용. UnityEvent에서 Action으로 수정
        //SerializedProperty m_OnClickLongProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");

            owner = (UIButton)target;
            //m_OnClickLongProperty = serializedObject.FindProperty(nameof(owner.OnLongClick));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_OnClickProperty);

            //// 추가
            //EditorGUILayout.PropertyField(m_OnClickLongProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}