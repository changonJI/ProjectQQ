using Cysharp.Threading.Tasks;
using QQ.FSM;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QQ
{
    public class Monster : SpineGameObject, IDamageable
    {
        public override GameObjectType ObjType => GameObjectType.Monster;

        private MonsterData monsterData;
        
        // 체력
        // private int maxHp() => monsterData.hp;
        private int maxHp() => 1;
        private int currentHp;
        
        public MonsterMovement MonsterMovement { get; private set; }
        
        private StatusEffectController.StatusEffect currentStatus = StatusEffectController.StatusEffect.None;

        // 이속
        public override float GetSpeed() => monsterData.speed + addSpeed;
        private float addSpeed = 0f;

        // 쿨타임
        private const float atkCoolTime = 1f;
        private float endAtkCoolTime = 0f;

        public Transform TargetTransform { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();

            SetLayer(gameObject, GameObjectType.Monster);
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

        protected override void OnCollisionEnter2Ded(Collision2D other)
        {
            if (other.gameObject.layer == (int)Layer.Player)
            {
                if (other.gameObject.TryGetComponent<IDamageable>(out var actor))
                {
                    float nowTime = Time.realtimeSinceStartup;
                    if (nowTime >= endAtkCoolTime)
                    {
                        actor.TakeDamage(monsterData.attack, transform.localPosition);
                        endAtkCoolTime = nowTime;
                    }
                }
            }
        }

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
                stateContext.ChangeState(stateContext.GetDieState());
                // Die();
            }
            else
            {
                // TODO: 맞는 이펙트, 넉백 등 추가 가능
            }
        }

        public async UniTaskVoid Die()
        {
            Debug.Log("몬스터 사망");

            // TODO: 사망 애니메이션 또는 효과 -> UniTask
            // TODO: 아이템 드랍
            await TryDropMonstItem();
            
            Destroy(gameObject);
            // PoolManager.Instance.ReleaseObject(this.gameObject);
        }

        private async UniTask TryDropMonstItem()
        {
            List<ItemData> candidates = ItemDataManager.Instance.GetAll()
                .Where(data => data.itemType == ItemType.Consumable)
                .ToList();

            // 랜덤 시드에 시간 요소를 더해 다양화
            float timeOffset = Time.time * 1000f;

            foreach (var item in candidates)
            {
                // 시간 offset으로 좌표 무작위성을 더함
                float randX = Mathf.PerlinNoise(item.id, timeOffset) - 0.5f;
                float randZ = Mathf.PerlinNoise(item.id + 999, timeOffset) - 0.5f;
                Vector3 dropPosition = transform.position + new Vector3(randX, 0, randZ);

                string prefabName = LanguageDataManager.Instance.Get(item.nameId, ConturyType.English);
                GameObject itemPrefab = await PoolManager.Instance.GetObject(GameObjectType.Item, prefabName, dropPosition, item.id);

                if (itemPrefab.TryGetComponent(out DropItem itemComponent))
                {
                    itemComponent.DataInit(item);
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
            MonsterMovement = gameObject.AddComponent<MonsterMovement>(this);
        }

        private void SetTable()
        {
            var data = MonsterDataManager.Instance.Get(tableID);

            monsterData.Set(data);
        }

        public void TakeStatus(SkillOptionType type, float value = 0)
        {
        }
    }
}