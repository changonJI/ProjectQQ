
namespace QQ
{
    public interface IDataManager
    {
        TableType GetTableType();
        void Clear();

        /// <summary>
        /// 자동으로 실행되는 메쏘드
        /// </summary>
        void SaveData(TableType type, string data) 
        {
            TableDataManager.SaveData(type, data); 
        }

        void LoadData();
    }
}