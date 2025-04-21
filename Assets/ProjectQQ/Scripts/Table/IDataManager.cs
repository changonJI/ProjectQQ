
namespace QQ
{
    public interface IDataManager
    {
        TableType GetTableType();
        void Clear();
        void SaveData(string data);
        void LoadData();
    }
}