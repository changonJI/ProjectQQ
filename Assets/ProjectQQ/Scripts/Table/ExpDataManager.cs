using System.Collections.Generic;

namespace QQ
{
    public class ExpDataManager : Singleton<ExpDataManager>, IDataManager
    {
        private readonly Dictionary<int, ExpData> dic_Data = new Dictionary<int, ExpData>();

        public TableType GetTableType() => TableType.ExpData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.ExpData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                ExpData data = new ExpData
                {
                    level = short.Parse(columns[0]),
                    NextLvExp = short.Parse(columns[1])
                };

                if (!dic_Data.ContainsKey(data.level))
                {
                    dic_Data.Add(data.level, data);
                }
            }
        }

        public ExpData Get(int id)
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
