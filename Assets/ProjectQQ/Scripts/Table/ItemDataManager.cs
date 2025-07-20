using System;
using System.Collections.Generic;
using System.Linq;

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
                    itemType = System.Enum.Parse<ItemType>(columns[1]),
                    nameId = int.Parse(columns[2]),
                    desId = int.Parse(columns[3]),
                    iconName = columns[4],
                    lvCount = short.Parse(columns[5]),
                    skillId = int.Parse(columns[6]),
                    boxValue = int.Parse(columns[7])
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
    }
}
