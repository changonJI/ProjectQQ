using TMPro;
using UnityEngine;

namespace QQ
{
    public class UIButtonAndText : MonoBehaviour
    {
        [SerializeField] private UIButton button;
        [SerializeField] private TextMeshProUGUI textMeshPro;

        public void SetText(string text)
        {
            textMeshPro.text = text;
        }

        public void OnClickAdd(System.Action action)
        {
            button.OnClickAction += action;
        }

        public void OnClickRemove(System.Action action)
        {
            button.OnClickAction -= action;
        }
    }
}