
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

        /*
        * 1. TableDataManager.LoadData() �� �Ű������� Enum TableType �Է�
        * 2. foreach ���� �ش��ϴ� Data Struct���� data ���ϰ� Manager�� Dictionary�� �����ϱ�
        */
        void LoadData();
    }
}