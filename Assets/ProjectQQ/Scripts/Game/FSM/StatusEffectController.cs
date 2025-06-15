using System;
using System.Collections.Generic;
using UnityEngine;

namespace QQ.FSM
{
    public class StatusEffectController
    {
        [Flags]
        public enum StatusEffect
        {
            None        = 0,
            Invincible  = 1 << 0,
            Poison      = 1 << 1,
            Stunned     = 1 << 2,
            Slowed      = 1 << 3,
            Silenced    = 1 << 4,
            Burning     = 1 << 5
        }
        
        private StatusEffect current = StatusEffect.None;
        private Dictionary<StatusEffect, float> timers = new();
        
        public event Action<StatusEffect> OnStatusApplied;
        public event Action<StatusEffect> OnStatusRemoved;
        
        private readonly List<StatusEffect> expiredBuffer = new();

        private void Update()
        {
            if(current == StatusEffect.None) return;
            
            // 상태이상 타이머
            expiredBuffer.Clear();

            foreach (var kvp in timers)
            {
                timers[kvp.Key] -= Time.deltaTime;
                if (timers[kvp.Key] <= 0)
                    expiredBuffer.Add(kvp.Key);
            }

            foreach (var effect in expiredBuffer)
            {
                RemoveStatus(effect);
            }
        }

        public void ApplyStatus(StatusEffect effect, float duration = -1f)
        {
            if (!HasStatus(effect))
            {
                current |= effect;
                OnStatusApplied?.Invoke(effect);
                ShowEffectVisual(effect);     // TODO : 이펙트 처리
                UpdateUI(effect, true);       // TODO : UI 표시
            }

            if (duration > 0f)
            {
                timers[effect] = duration;
            }
        }

        public void RemoveStatus(StatusEffect effect)
        {
            if (HasStatus(effect))
            {
                current &= ~effect;
                timers.Remove(effect);
                OnStatusRemoved?.Invoke(effect);
                HideEffectVisual(effect);     // TODO : 이펙트 제거
                UpdateUI(effect, false);      // TODO : UI 제거
            }
        }

        public bool HasStatus(StatusEffect effect)
        {
            return (current & effect) != 0;
        }

        private void ShowEffectVisual(StatusEffect effect)
        {
            // 예: particleSystem.Play() or 이펙트 매니저 호출
        }

        private void HideEffectVisual(StatusEffect effect)
        {
            // 예: particleSystem.Stop() or 오브젝트 비활성화
        }

        private void UpdateUI(StatusEffect effect, bool enable)
        {
            // 예: 상태이상 아이콘 on/off
        }
    }
}