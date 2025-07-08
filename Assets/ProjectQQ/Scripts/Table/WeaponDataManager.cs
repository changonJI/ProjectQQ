using System.Collections.Generic;

namespace QQ
{
    public class WeaponDataManager : Singleton<WeaponDataManager>, IDataManager
    {
        private readonly Dictionary<int, WeaponData> dic_Data = new Dictionary<int, WeaponData>();

        public TableType GetTableType() => TableType.WeaponData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.WeaponData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                WeaponData data = new WeaponData
                {
                    id = short.Parse(columns[0]),
                    nameId = int.Parse(columns[1]),
                    desTextId = int.Parse(columns[2]),
                    skillId = int.Parse(columns[3]),
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public WeaponData Get(int id)
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
