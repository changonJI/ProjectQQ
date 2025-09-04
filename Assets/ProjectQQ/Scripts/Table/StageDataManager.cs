using System.Collections.Generic;

namespace QQ
{
    public class StageDataManager : Singleton<StageDataManager>, IDataManager
    {
        private readonly Dictionary<int, StageData> dic_Data = new Dictionary<int, StageData>();

        public TableType GetTableType() => TableType.StageData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.StageData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                StageData data = new StageData
                {
                    id = int.Parse(columns[0]),
                    chapterGroup = int.Parse(columns[1]),
                    chapterNameId = int.Parse(columns[2]),
                    stageGroup = int.Parse(columns[3]),
                    stageNameId = int.Parse(columns[4]),
                    stagNameDes = int.Parse(columns[5]),
                    bgmId = columns[6],
                    backgroundAsset = columns[7],
                    spawnTableId = int.Parse(columns[11]),
                    dropTableId = int.Parse(columns[12]),
                    isBossStage = bool.Parse(columns[13]),
                    stageFxId = columns[10],
                    stageNumber = int.Parse(columns[14])
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }
    }
}
