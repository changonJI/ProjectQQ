using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QQ
{
    public class ItemDataManager : Singleton<ItemDataManager>, IDataManager
    {
        private readonly Dictionary<int, ItemData> dic_Data = new Dictionary<int, ItemData>();

        public TableType GetTableType() => TableType.ItemData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.ItemData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                ItemData data = new ItemData
                {
                    id = short.Parse(columns[0]),
                    itemType = (ItemType)int.Parse(columns[1]),
                    nameId = int.Parse(columns[2]),
                    desId = int.Parse(columns[3]),
                    iconName = columns[4],
                    lvCount = short.Parse(columns[5]),
                    skillId = int.Parse(columns[6]),
                    targetType = short.Parse(columns[7]),
                    nextID = int.Parse(columns[8]),
                    originType = int.Parse(columns[9]),
                    isShopItem = ParseBool(columns[10]),
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public ItemData Get(int id)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                return dic_Data[id];
            }
            else
            {
                LogHelper.LogError($"PlayerStatData is Null : {id}");
                return default;
            }
        }

        public List<ItemData> GetRandomItems(int count)
        {
            if (dic_Data.Count == 0 || count <= 0)
                return new List<ItemData>();

            return dic_Data.Values
                .OrderBy(_ => Guid.NewGuid()) // 무작위 정렬
                .Take(count)
                .ToList();
        }

        public List<ItemData> GetAll()
        {
            return dic_Data.Values.ToList();
        }

        private bool ParseBool(string value)
        {
            return value.Equals("true", StringComparison.OrdinalIgnoreCase)
                   || value.Equals("TRUE", StringComparison.OrdinalIgnoreCase)
                   || value.Equals("1");
        }

        public List<ItemData> GetRandomRouletteItems(int minCount = 3, int maxCount = 5)
        {
            // 0) 테이블 로드: Consumable + Attack만
            var all = GetAll()
                .Where(i => i.id > 0 && (i.itemType == ItemType.Consumable || i.itemType == ItemType.Attack))
                .GroupBy(i => i.id).Select(g => g.First()) // DistinctBy(id) 대체
                .ToList();

            var byId = all.ToDictionary(i => i.id, i => i);
            var consumablePool = all.Where(i => i.itemType == ItemType.Consumable).ToList();

            // 인벤토리
            var inventory = PoolManager.Instance.actor.GetInventory(); // List<ItemData>
            int ownedAttackCount = inventory.Count(it => it.itemType == ItemType.Attack);

            // 보유 공격의 originType 집합
            var ownedOrigins = new HashSet<int>(
                inventory.Where(it => it.itemType == ItemType.Attack).Select(it => it.originType)
            );

            // 진화 가능한 보유 공격(= nextID > 0 이고 next가 Attack)
            var evolvableAttacks = inventory
                .Where(it => it.itemType == ItemType.Attack && it.nextID > 0)
                .Where(it => byId.TryGetValue((short)it.nextID, out var next) && next.itemType == ItemType.Attack)
                .ToList();

            bool hasEvolvable = evolvableAttacks.Count > 0;

            // 보유하지 않은 originType의 1레벨 Attack 풀 (모두 만렙인 경우에만 사용)
            var level1OtherOriginPool = all
                .Where(i => i.itemType == ItemType.Attack && i.lvCount == 1 && !ownedOrigins.Contains(i.originType))
                .ToList();

            // 1) 총 개수 결정
            int totalCount = UnityEngine.Random.Range(minCount, maxCount + 1);

            // 2) 공격/소모품 슬롯 수 결정 (제약 반영)
            int attackSlots = 0;
            if (hasEvolvable)
            {
                // 공격 슬롯 상한: 보유 공격 개수, evolvable 개수, (소모품 최소 1개 유지 시) totalCount - 1
                int upper = Math.Min(ownedAttackCount, evolvableAttacks.Count);
                if (consumablePool.Count > 0) upper = Math.Min(upper, totalCount - 1);

                // 하한 = 1 (가능할 때)
                int lower = Math.Min(1, upper);
                attackSlots = (upper >= lower) ? UnityEngine.Random.Range(lower, upper + 1) : 0;
            }
            else
            {
                // 모든 보유 공격이 최대 레벨 → 다른 originType의 Lv1 Attack을 "딱 1개"만 (가능할 때)
                bool canPlaceOne = level1OtherOriginPool.Count > 0 && ownedAttackCount > 0;
                // 소모품도 보여줄 여지 고려(있으면 최소 1개 남겨두기)
                if (canPlaceOne)
                {
                    if (consumablePool.Count > 0)
                        attackSlots = (totalCount >= 2) ? 1 : 0; // 최소 1 소비 보장
                    else
                        attackSlots = (totalCount >= 1) ? 1 : 0; // 소비가 아예 없으면 1도 허용
                }
                else
                {
                    attackSlots = 0;
                }
            }

            int consumableSlots = Mathf.Clamp(totalCount - attackSlots, 0, totalCount);

            // 3) 카테고리 플랜 구성
            var categoryPlan = new List<ItemType>(totalCount);
            for (int i = 0; i < attackSlots; i++) categoryPlan.Add(ItemType.Attack);
            for (int i = 0; i < consumableSlots; i++) categoryPlan.Add(ItemType.Consumable);
            ShuffleInPlace(categoryPlan);

            // 4) 슬롯 채우기
            var result = new List<ItemData>(totalCount);
            var taken = new HashSet<short>();

            foreach (var cat in categoryPlan)
            {
                ItemData pick;

                if (cat == ItemType.Attack)
                {
                    if (hasEvolvable)
                    {
                        // 보유 공격의 next만
                        if (PickRandomAttackNext(evolvableAttacks, byId, taken, out pick))
                        {
                            result.Add(pick);
                            taken.Add(pick.id);
                            continue;
                        }
                    }
                    else
                    {
                        // 모두 만렙 → 다른 originType의 1레벨 Attack 1개
                        if (PickRandomLevel1OtherOrigin(level1OtherOriginPool, taken, out pick))
                        {
                            result.Add(pick);
                            taken.Add(pick.id);
                            continue;
                        }
                    }

                    // 폴백: 소모품
                    if (PickRandomConsumable(consumablePool, taken, out pick))
                    {
                        result.Add(pick);
                        taken.Add(pick.id);
                        continue;
                    }
                }
                else
                {
                    if (PickRandomConsumable(consumablePool, taken, out pick))
                    {
                        result.Add(pick);
                        taken.Add(pick.id);
                        continue;
                    }

                    // 소비 고갈 폴백
                    if (hasEvolvable && PickRandomAttackNext(evolvableAttacks, byId, taken, out pick))
                    {
                        result.Add(pick);
                        taken.Add(pick.id);
                        continue;
                    }

                    if (!hasEvolvable && PickRandomLevel1OtherOrigin(level1OtherOriginPool, taken, out pick))
                    {
                        result.Add(pick);
                        taken.Add(pick.id);
                        continue;
                    }
                }

                // 풀 고갈 시 종료
                break;
            }

            // 5) 최종 셔플
            ShuffleInPlace(result);
            return result;
        }

        private static bool PickRandomConsumable(List<ItemData> consumablePool, HashSet<short> taken, out ItemData pick)
        {
            pick = default;
            if (consumablePool == null || consumablePool.Count == 0) return false;

            var remain = consumablePool.Where(c => !taken.Contains(c.id)).ToList();
            if (remain.Count == 0) return false;

            pick = remain[UnityEngine.Random.Range(0, remain.Count)];
            return true;
        }

        private static bool PickRandomAttackNext(List<ItemData> evolvableAttacks,
            Dictionary<short, ItemData> byId,
            HashSet<short> taken,
            out ItemData pick)
        {
            pick = default;
            if (evolvableAttacks == null || evolvableAttacks.Count == 0) return false;

            int guard = 0; // 중복 회피 시도 제한
            while (guard++ < 16)
            {
                var owned = evolvableAttacks[UnityEngine.Random.Range(0, evolvableAttacks.Count)];
                short nextId = (short)owned.nextID;

                if (nextId <= 0) continue;
                if (!byId.TryGetValue(nextId, out var nextItem)) continue;
                if (nextItem.itemType != ItemType.Attack) continue;
                if (taken.Contains(nextItem.id)) continue;

                pick = nextItem;
                return true;
            }

            return false;
        }

        private static bool PickRandomLevel1OtherOrigin(List<ItemData> level1Pool,
            HashSet<short> taken,
            out ItemData pick)
        {
            pick = default;
            if (level1Pool == null || level1Pool.Count == 0) return false;

            var remain = level1Pool.Where(a => !taken.Contains(a.id)).ToList();
            if (remain.Count == 0) return false;

            pick = remain[UnityEngine.Random.Range(0, remain.Count)];
            return true;
        }

        private static void ShuffleInPlace<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}