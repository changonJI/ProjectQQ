using System.Collections.Generic;

namespace QQ
{
    public class LanguageDataManager : Singleton<LanguageDataManager>, IDataManager
    {
        private readonly Dictionary<int, LanguageData> dic_Data = new Dictionary<int, LanguageData>();

        public TableType GetTableType() => TableType.LanguageData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.LanguageData);

            foreach (string str in dataRows) 
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                LanguageData data = new LanguageData() 
                { 
                    ID = int.Parse(columns[0]),
                };

                if (!dic_Data.ContainsKey(data.ID))
                {
                    dic_Data.Add(data.ID, data);
                }
            }
        }

        public LanguageData Get(int id)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                return dic_Data[id];
            }
            else
            {
                LogHelper.LogError($"LanguageDat is Null : {id}");

                return default;
            }
        }
    }
}

