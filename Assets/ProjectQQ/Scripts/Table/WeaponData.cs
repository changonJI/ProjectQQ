public struct WeaponData : IData
{
    public short id;    // key
    /// <summary>
    /// 언어테이블 참조 값
    /// </summary>
    public int nameId;   // 무기 이름
    /// <summary>
    /// 언어테이블 참조 값
    /// </summary>
    public int desTextId;  // 무기 설명
    /// <summary>
    /// 스킬테이블 참조 값
    /// </summary>
    public int skillId;  // 스킬 ID

    public void Clear()
    {
        id = 0;
        nameId = 0;
        desTextId = 0;
        skillId = 0;
    }
}
