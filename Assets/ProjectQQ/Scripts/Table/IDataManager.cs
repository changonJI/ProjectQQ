
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

        /*
        * 1. TableDataManager.LoadData() 에 매개변수값 Enum TableType 입력
        * 2. foreach 에서 해당하는 Data Struct값에 data 셋하고 Manager의 Dictionary에 저장하기
        */
        void LoadData();
    }
}