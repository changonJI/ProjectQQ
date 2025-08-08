using System;
using System.Collections.Generic;

namespace QQ
{
    public class SkillDataManager : Singleton<SkillDataManager>, IDataManager
    {
        private readonly Dictionary<int, SkillData> dic_Data = new Dictionary<int, SkillData>();

        public TableType GetTableType() => TableType.SkillData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.SkillData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                SkillData data = new SkillData
                {
                    id = int.Parse(columns[0]),
                    nameId = int.Parse(columns[1]),
                    type = (SkillType)Enum.Parse(typeof(SkillType), columns[2]),
                    desId = int.Parse(columns[3]),
                    prefabName = columns[4],
                    soundName = columns[5],
                    cooltime = int.Parse(columns[6]),
                    duration = int.Parse(columns[7]),
                    range = int.Parse(columns[8]),
                    speed = int.Parse(columns[9]),
                    blowCount = int.Parse(columns[10]),
                    mainOptionType = (SkillOptionType)Enum.Parse(typeof(SkillOptionType), columns[11]),
                    mainOptionValue = int.Parse(columns[12]),
                    subOptionType = (SkillOptionType)Enum.Parse(typeof(SkillOptionType), columns[13]),
                    subOptionValue = int.Parse(columns[14])
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public SkillData Get(int id)
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
