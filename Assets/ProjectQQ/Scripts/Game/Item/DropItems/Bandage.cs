using QQ;
using UnityEngine;

namespace ProjectQQ.Scripts.Game.Item.DropItems
{
    public class Bandage : DropItem
    {
        protected override void OnUseItem(Actor actor)
        {
            Debug.Log($"Bandage used: +{actor.name}");
            
            int healAmount = 1; // 수정 :: 데이터 연결
            
            // 플레이어 객체 참조
            actor.Heal(healAmount);

            PoolManager.Instance.ReleaseObject(this.gameObject);
        }
    }
}