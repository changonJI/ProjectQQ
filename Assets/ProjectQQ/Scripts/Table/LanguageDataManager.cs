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
                    id = int.Parse(columns[0]),
                    korean = columns[1],
                    english = columns[2],
                    chinese = columns[3],
                    japanese = columns[4]
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public string Get(int id, ConturyType type)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                switch (type)
                {
                    case ConturyType.English:
                        return dic_Data[id].english;
                    case ConturyType.Chinese:
                        return dic_Data[id].chinese;
                    case ConturyType.Japanese:
                        return dic_Data[id].japanese;
                    case ConturyType.Korean:
                    default:
                        return dic_Data[id].korean;
                }
            }
            else
            {
                LogHelper.LogError($"LanguageDat is Null : {id}");

                return string.Empty;
            }
        }
    }
}

