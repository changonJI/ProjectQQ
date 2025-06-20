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
        /// 버튼 범위 안에서 마우스를 누르는 순간의 이벤트
        /// </summary>
        public override void OnPointerDown(PointerEventData eventData)
        {
            SetTime(0f);
            SetPress(true);
        }

        /// <summary>
        /// 버튼 범위 상관없이 마우스를 누르고 땠을 때 발생하는 이벤트
        /// </summary>
        public override void OnPointerUp(PointerEventData eventData)
        {
            SetPress(false);
        }

        /// <summary>
        /// 버튼 범위 안에서 마우스를 누르고 땟을 때 발생하는 이벤트
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

            // 사운드 재생
            SoundManager.Instance.PlayUI(audioClip);

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