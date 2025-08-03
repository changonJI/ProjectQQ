using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using QQ.FSM;
using UnityEngine;

namespace QQ
{
    public class Monster : SpineGameObject, IDamageable
    {
        public override GameObjectType Type => GameObjectType.Monster;

        private MonsterData monsterData;
        
        // 체력
        private int maxHp() => monsterData.hp;
        private int currentHp;
        
        public MonsterMovement MonsterMovement { get; private set; }
        
        private StatusEffectController.StatusEffect currentStatus = StatusEffectController.StatusEffect.None;
        
        public Transform TargetTransform { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            InitMonster();
            InitController();
        }

        protected override void OnStart()
        {
            SetTable();
            currentHp = maxHp();
            
            stateContext.ChangeState(stateContext.GetIdleState());

            TryFindPlayer();
        }

        protected override void OnUpdate()
        {
            TryFindPlayer();
            stateContext.Update();
        }

        protected override void OnFixedUpdate()
        {
        }
        protected override void OnLateUpdate() { }
        protected override void OnDestroyed() { }

        public void TryFindPlayer()
        {
            if (TargetTransform == null)
            {
                var player = PoolManager.Instance.actor;
                if (player != null) //  && !player.IsDead
                {
                    TargetTransform = player.transform;
                }
            }
        }

        public void TakeDamage(int damage, Vector3 transformPosition)
        {
            currentHp -= damage;
            Debug.Log($"몬스터 피격: -{damage}, 현재 체력: {currentHp}");

            if (currentHp <= 0)
            {
                Die();
            }
            else
            {
                // TODO: 맞는 이펙트, 넉백 등 추가 가능
            }
        }

        private void Die()
        {
            Debug.Log("몬스터 사망");

            // TODO: 사망 애니메이션 또는 효과 -> UniTask
            // TODO: 아이템 드랍
            TryDropItem();
            
            PoolManager.Instance.ReleaseObject(this.gameObject);
        }

        private async UniTaskVoid TryDropItem()
        {
            // 1. 소비형 아이템만 필터링
            List<ItemData> candidates = ItemDataManager.Instance.GetAll()
                .Where(data => data.itemType == ItemType.Consumable) // item_type == 1
                .ToList();

            foreach (var item in candidates)
            {
                // 2. 드랍 확률 검사
                if (UnityEngine.Random.value <= item.dropChance)
                {
                    // 3. 아이템 드랍
                    GameObject dropObj = await PoolManager.Instance.GetObject(GameObjectType.Item, "DropItem", 5);
                    dropObj.transform.position = transform.position;

                    if (dropObj.TryGetComponent(out DropItem dropItem))
                    {
                        dropItem.DataInit(item);
                    }

                    Debug.Log($"[드랍 성공] 몬스터가 {item.id} 아이템을 드랍했습니다.");
                    break; // 한 개만 드랍 후 종료
                }
            }
        }

        private void InitMonster()
        {
            monsterData = new MonsterData();
            stateContext = new MonsterStateContext(this);
            
        }

        private void InitController()
        {
            MonsterMovement = GetComponent<MonsterMovement>();
        }

        private void SetTable()
        {
            var data = MonsterDataManager.Instance.Get(tableID);

            monsterData.Set(data);
        }
    }
}