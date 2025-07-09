using System.Collections.Generic;

namespace QQ
{
    public class BaseDataManager : Singleton<BaseDataManager>, IDataManager
    {
        private readonly Dictionary<int, BaseData> dic_Data = new Dictionary<int, BaseData>();

        public TableType GetTableType() => TableType.BaseData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.BaseData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                BaseData data = new BaseData
                {
                    id = short.Parse(columns[0]),
                    unit = short.Parse(columns[1]),
                };
                
                switch(data.unit)
                {
                    case 0: // bool
                        data.data = bool.Parse(columns[2]);
                        break;
                    case 1: // int
                        data.data = int.Parse(columns[2]);
                        break;
                    case 2: // 백분율
                        data.data =  float.Parse(columns[2]);
                        break;
                    case 3: // 천분율
                        data.data = float.Parse(columns[2]);
                        break;
                    case 4: // 만분율
                        data.data = float.Parse(columns[2]);
                        break;
                    case 5: // string
                        data.data = columns[2];
                        break;
                    default:
                        LogHelper.LogError($"Unknown unit type: {data.unit}");
                        break;
                }

                data.a = columns[3];

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public int GetInt(int id)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                return (int)dic_Data[id].data;
            }
            else
            {
                LogHelper.LogError($"BaseData is Null : {id}");
                return default;
            }
        }

        public float Getfloat(int id)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                return (float)dic_Data[id].data;
            }
            else
            {
                LogHelper.LogError($"BaseData is Null : {id}");
                return default;
            }
        }

        public string GetString(int id)
        {
            if (id <= 0) return default;

            if (dic_Data.ContainsKey(id))
            {
                return dic_Data[id].data.ToString();
            }
            else
            {
                LogHelper.LogError($"BaseData is Null : {id}");
                return default;
            }
        }
    }
}
