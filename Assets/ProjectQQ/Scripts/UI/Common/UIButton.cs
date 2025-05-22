using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QQ
{
    /// <summary>
    /// <see cref="UIButtonEditor"/>
    /// </summary>
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("UI/UIButton")]
    public class UIButton : Button
    {
        public string audioClip = "ButtonClick";

        private System.Action OnClick;
        private System.Action OnLongClick;

        private bool isPressed = false;
        private float pressTime = 0f;
        private float longClickTime = 1.0f;

        public System.Action OnClickAction
        {
            get => OnClick;
            set => OnClick += value;
        }

        public System.Action OnLongClickAction
        {
            get => OnLongClick;
            set => OnLongClick += value;
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
                OnLongClick?.Invoke();
            }
            else
            {
                OnClick?.Invoke();
            }

            // ���� ���
            SoundManager.Instance.PlaySFX(audioClip);

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
}