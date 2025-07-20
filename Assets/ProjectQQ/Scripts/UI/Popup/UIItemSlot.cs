using System;
using QQ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectQQ.Scripts.UI.Popup
{
    public class UIItemSlot : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI simpleDescriptionText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetData(ItemData data)
        {
            iconImage.sprite = Resources.Load<Sprite>($"Icon/{data.iconName}");
            nameText.text = data.nameId.ToText();
            simpleDescriptionText.text = $"아이템 타입 : {data.itemType.ToString()} \n 아이템 등급 : 어떤거참조?";
            descriptionText.text = data.desId.ToText();
        }
    }
}