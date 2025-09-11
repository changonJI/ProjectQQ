using System.Collections.Generic;

namespace QQ
{
    public class MonsterSpawnDataManager : Singleton<MonsterSpawnDataManager>, IDataManager
    {
        private readonly Dictionary<int, MonsterSpawnData> dic_Data = new Dictionary<int, MonsterSpawnData>();

        public TableType GetTableType() => TableType.MonsterSpawnData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.MonsterSpawnData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                MonsterSpawnData data = new MonsterSpawnData
                {
                    id = int.Parse(columns[0]),    // 웨이브 아이디
                    spawnStart = float.Parse(columns[1]),    // spawn 시작 시간
                    spawnLoop = float.Parse(columns[2]),    // spawn 반복 시간
                    spawnCount = int.Parse(columns[3]),    // spawn 횟수
                    monsterId1 = int.Parse(columns[4]),    // monster ID 1
                    monsterCnt1 = short.Parse(columns[5]),  // monster 마릿수 1
                    monsterId2 = int.Parse(columns[6]),    // monster ID 2
                    monsterCnt2 = short.Parse(columns[7])   // monster 마릿수 2
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public MonsterSpawnData Get(int id)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                return dic_Data[id];
            }
            else
            {
                LogHelper.LogError($"MonsterSpawnData is Null : {id}");
                return default;
            }
        }
    }
}
