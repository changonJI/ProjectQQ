public struct ExpData : IData
{
    public short id;    // key
    public short level;   // 레벨
    public short NextLvExp;  // 다음 레벨 경험치

    public void Clear()
    {
        id = 0;
        level = 0;
        NextLvExp = 0;
    }
}
