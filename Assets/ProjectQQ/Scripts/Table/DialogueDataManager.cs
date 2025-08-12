using System.Collections.Generic;

namespace QQ
{
    public class DialogueDataManager : Singleton<DialogueDataManager>, IDataManager
    {
        private readonly Dictionary<int, DialogueData> dic_Data = new Dictionary<int, DialogueData>();

        public TableType GetTableType() => TableType.DialogueData;

        public void Clear()
        {
            dic_Data.Clear();
        }

        public void LoadData()
        {
            string[] dataRows = TableDataManager.LoadData(TableType.DialogueData);

            foreach (string str in dataRows)
            {
                string[] columns = str.Split('\t');

                // key값 비어있으면 넘김
                if (string.IsNullOrEmpty(columns[0]))
                    continue;

                DialogueData data = new DialogueData
                {
                    id              = short.Parse(columns[0]),
                    dialogueGroup   = short.Parse(columns[1]),
                    speakerType     = byte.Parse(columns[2]),
                    speakerNameId   = int.Parse(columns[3]),
                    dialogueTextId  = int.Parse(columns[4]),
                    emotionType     = byte.Parse(columns[5]),
                    portraitId      = columns[6],
                    voiceClipId     = columns[7],
                    nextDialogueId  = short.Parse(columns[8]),
                    triggerEvent    = columns[9],
                    autoAdvance     = bool.Parse(columns[10]),
                    advanceDelay    = float.Parse(columns[11])
                };

                if (!dic_Data.ContainsKey(data.id))
                {
                    dic_Data.Add(data.id, data);
                }
            }
        }

        public DialogueData Get(int id)
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
