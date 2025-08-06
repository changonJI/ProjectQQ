public struct MonsterSpawnData : IData
{
    public int id;   // 웨이브 아이디(100101 => 앞자리부터 10 챕터, 01 스테이지, 01 웨이브 번호)
    public int spawnStart;  // spawn 시작 시간
    public int spawnLoop;  // spawn 반복 시간
    public int spawnCount;  // spawn 횟수
    /// <summary>
    /// 몬스터 테이블 참조
    /// </summary>
    public int monsterId1;  // monster ID
    public short monsterCnt1;  // monster 마릿수
    /// <summary>
    /// 몬스터 테이블 참조
    /// </summary>
    public int monsterId2;  // monster ID
    public short monsterCnt2;  // monster 마릿수

    public void Clear()
    {
        id = 0;
        spawnStart = 0;
        spawnLoop = 0;
        spawnCount = 0;
        monsterId1 = 0;
        monsterCnt1 = 0;
        monsterId2 = 0;
        monsterCnt2 = 0;
    }
}
