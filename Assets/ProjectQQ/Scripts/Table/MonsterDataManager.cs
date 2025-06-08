using System.Collections.Generic;

namespace QQ
{
    public class MonsterDataManager : Singleton<MonsterDataManager>, IDataManager
    {
        private readonly Dictionary<int, MonsterData> dic_Data = new Dictionary<int, MonsterData>();

        public TableType GetTableType() => TableType.MonsterData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.MonsterData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                MonsterData data = new MonsterData
                {
                    id = short.Parse(columns[0]),
                    nameId = int.Parse(columns[1]),
                    desId = int.Parse(columns[2]),
                    type = (MonsterType)int.Parse(columns[3]),
                    hp = short.Parse(columns[4]),
                    attack = short.Parse(columns[5]),
                    atkType = (MonsterAtkType)int.Parse(columns[6]),
                    skill = short.Parse(columns[7]),
                    attackLag = float.Parse(columns[8]),
                    attackAng = float.Parse(columns[9]),
                    speed = float.Parse(columns[10]),
                    spriteName = columns[11],
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public MonsterData Get(int id)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                return dic_Data[id];
            }
            else
            {
                LogHelper.LogError($"MonsterData is Null : {id}");
                return default;
            }
        }
    }
}
