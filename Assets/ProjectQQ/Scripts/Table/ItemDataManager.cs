using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

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
                    // skillId = int.Parse(columns[6]),
                    targetType = short.Parse(columns[7]),
                    nextID = int.Parse(columns[8]),
                    isShopItem = ParseBool(columns[9]),
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
            List<ItemData> allItems = GetAll()
                .Where(item => item.id > 0)
                .DistinctBy(item => item.id)
                .ToList();

            var groupedByType = allItems
                .GroupBy(item => item.itemType)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<ItemData> result = new List<ItemData>();

            // Step 1: 각 itemType에서 하나씩 선택
            foreach (var kv in groupedByType)
            {
                var itemsOfType = kv.Value;
                if (itemsOfType.Count > 0)
                {
                    var randomItem = itemsOfType[UnityEngine.Random.Range(0, itemsOfType.Count)];
                    result.Add(randomItem);
                }
            }

            // Step 2: 추가 아이템을 무작위로 넣되, 중복 피함
            int maxSlotCount = UnityEngine.Random.Range(minCount, maxCount + 1);

            var remainingCandidates = allItems
                .Where(item => !result.Contains(item))
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

            foreach (var item in remainingCandidates)
            {
                if (result.Count >= maxSlotCount)
                    break;

                result.Add(item);
            }

            // 최종 정렬 (옵션)
            return result.OrderBy(_ => Guid.NewGuid()).ToList();
        }
    }
}