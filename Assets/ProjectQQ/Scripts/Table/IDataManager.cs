
namespace QQ
{
    public interface IDataManager
    {
        TableType GetTableType();
        void Clear();

        /// <summary>
        /// �ڵ����� ����Ǵ� �޽��
        /// </summary>
        void SaveData(TableType type, string data) 
        {
            TableDataManager.SaveData(type, data); 
        }

        void LoadData();
    }
}