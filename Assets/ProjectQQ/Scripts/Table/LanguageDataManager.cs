using System.Collections.Generic;
using System.Linq;

namespace QQ
{
    public class LanguageDataManager : Singleton<LanguageDataManager>, IDataManager
    {
        private readonly Dictionary<int, LanguageData> dic_Data = new Dictionary<int, LanguageData>();

        public TableType GetTableType() => TableType.Language;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void SaveData(string str_Data)
        {
            TableData.SaveData(nameof(LanguageData), str_Data);
        }

        public void LoadData()
        {
            string str_Data = TableData.LoadData(nameof(LanguageData));

            string[] rows = str_Data.Split('\n');

            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split('\t');
                LanguageData data = new LanguageData();

                data.SetData(columns.ToList());

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
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"LanguageDat is Null : {id}");
#endif
                return default;
            }
        }
    }
}

