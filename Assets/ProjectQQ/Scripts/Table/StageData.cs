public struct StageData : IData
{
    public int id;                      // key
    public int chapterGroup;
    public int chapterNameId;
    public int stageGroup;
    public int stageNameId;            // 스테이지 이름 텍스트 ID
    public int stagNameDes;            // 스테이지 설명 텍스트 ID
    public string bgmId;               // 배경 음악 ID
    public string backgroundAsset;     // 배경 에셋
    public int spawnTableId;           // 스폰 테이블 ID
    public int dropTableId;            // 드롭 테이블 ID
    public bool isBossStage;           // 보스 스테이지 여부
    public string stageFxId;           // 스테이지 FX ID
    public int stageNumber;            // 스테이지 번호

    public void Clear()
    {
        id = 0;
        chapterGroup = 0;
        chapterNameId = 0;
        stageGroup = 0;
        stageNameId = 0;
        stagNameDes = 0;
        bgmId = string.Empty;
        backgroundAsset = string.Empty;
        spawnTableId = 0;
        dropTableId = 0;
        isBossStage = false;
        stageFxId = string.Empty;
        stageNumber = 0;
    }
}
