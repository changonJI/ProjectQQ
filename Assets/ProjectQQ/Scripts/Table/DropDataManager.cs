using System.Collections.Generic;

namespace QQ
{
    public class DropDataManager : Singleton<DropDataManager>, IDataManager
    {
        private readonly Dictionary<int, DropData> dic_Data = new Dictionary<int, DropData>();

        public TableType GetTableType() => TableType.DropData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.DropData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                DropData data = new DropData
                {
                    id = short.Parse(columns[0]), // Drop ID
                    monsterId = int.Parse(columns[1]), // 몬스터 ID
                    item1 = new DropItemData
                    {
                        itemId = int.Parse(columns[2]),
                        dropRate = float.Parse(columns[3]),
                        dropCount = short.Parse(columns[4]),
                    }, // 아이템 1
                    item2 = new DropItemData
                    {
                        itemId = int.Parse(columns[5]),
                        dropRate = float.Parse(columns[6]),
                        dropCount = short.Parse(columns[7]),
                    }, // 아이템 1
                    item3 = new DropItemData
                    {
                        itemId = int.Parse(columns[8]),
                        dropRate = float.Parse(columns[9]),
                        dropCount = short.Parse(columns[10]),
                    }, // 아이템 1

                    isBoss = int.Parse(columns[11]) > 0 // 보스 여부
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public DropData Get(int id)
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
    }
}
