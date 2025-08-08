using QQ;
using UnityEngine;

namespace ProjectQQ.Scripts.Game.Item.DropItems
{
    public class Serum : DropItem
    {
        protected override void OnUseItem(Actor actor)
        {
            int expAmount = 1; // 수정 :: 데이터 연결
            Debug.Log($"Serum used: +{expAmount} EXP");

            actor.AddExp(expAmount);
            
            PoolManager.Instance.ReleaseObject(this.gameObject);
        }
    }
}