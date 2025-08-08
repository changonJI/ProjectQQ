using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QQ
{
    public class SkillManager : DontDestroySingleton<SkillManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void Init() { }

        /// <summary>
        /// 스킬 사용
        /// </summary>
        /// <param name="skillID">사용 하는 스킬 ID값</param>
        /// <param name="spawnPos">사용 하는 스킬의 위치 값</param>
        public async UniTaskVoid UseSkill(int skillID, Vector3 spawnPos)
        {
            var data = SkillDataManager.Instance.Get(skillID);

            if (PoolManager.IsValid())
            {
                var skill = await PoolManager.Instance.GetObject(GameObjectType.SFX, data.prefabName, spawnPos, skillID);
            }
        }
    }
}