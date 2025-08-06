using System.Collections.Generic;

namespace QQ
{
    public class PlayerStatDataManager : Singleton<PlayerStatDataManager>, IDataManager
    {
        private readonly Dictionary<int, PlayerStatData> dic_Data = new Dictionary<int, PlayerStatData>();

        public TableType GetTableType() => TableType.PlayerStatData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.PlayerStatData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                PlayerStatData data = new PlayerStatData
                {
                    playerLevel = short.Parse(columns[0]),
                    heartMax = short.Parse(columns[1]),
                    baseAttack = short.Parse(columns[2]),
                    baseSpeed = float.Parse(columns[3]),
                    dodgeCooldown = float.Parse(columns[4]),
                };

                if (!dic_Data.ContainsKey(data.playerLevel))
                {
                    dic_Data.Add(data.playerLevel, data);
                }
            }
        }

        public PlayerStatData Get(int id)
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
