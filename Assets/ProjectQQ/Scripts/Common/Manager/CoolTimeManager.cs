using System.Collections.Generic;
using UnityEngine;

namespace QQ
{
    public class CoolTimeManager : Singleton<CoolTimeManager>
    {
        private Dictionary<int, float> cooldownEnd = new();

        public bool IsItemReady(int skillId, float duration)
        {
            float nowTime = Time.realtimeSinceStartup;
            bool hasCool = cooldownEnd.TryGetValue(skillId, out var endTime);

            if (!hasCool)
            {
                // 딕셔너리에 키가 없으면 바로 쿨다운 시작
                cooldownEnd[skillId] = nowTime + duration;
                return true;
            }

            if (nowTime >= endTime)
            {
                // 딕셔너리에 키는 있지만, 쿨다운 시간이 지났으면 다시 시작
                cooldownEnd[skillId] = nowTime + duration;
                return true;
            }

            // 키가 있고, 아직 쿨다운이 끝나지 않은 경우
            return false;
        }
    }
}
