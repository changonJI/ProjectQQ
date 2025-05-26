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

        /*
         * 1. TableDataManager.LoadData() 에 매개변수값 Enum TableType 입력
         * 2. foreach 에서 해당하는 Data Struct값에 data 셋하고 Manager의 Dictionary에 저장하기
         */
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
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"LanguageDat is Null : {id}");
#endif
                return default;
            }
        }
    }
}

