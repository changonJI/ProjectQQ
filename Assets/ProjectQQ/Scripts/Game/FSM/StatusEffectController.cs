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
            
            // �����̻� Ÿ�̸�
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
                ShowEffectVisual(effect);     // TODO : ����Ʈ ó��
                UpdateUI(effect, true);       // TODO : UI ǥ��
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
                HideEffectVisual(effect);     // TODO : ����Ʈ ����
                UpdateUI(effect, false);      // TODO : UI ����
            }
        }

        public bool HasStatus(StatusEffect effect)
        {
            return (current & effect) != 0;
        }

        private void ShowEffectVisual(StatusEffect effect)
        {
            // ��: particleSystem.Play() or ����Ʈ �Ŵ��� ȣ��
        }

        private void HideEffectVisual(StatusEffect effect)
        {
            // ��: particleSystem.Stop() or ������Ʈ ��Ȱ��ȭ
        }

        private void UpdateUI(StatusEffect effect, bool enable)
        {
            // ��: �����̻� ������ on/off
        }
    }
}