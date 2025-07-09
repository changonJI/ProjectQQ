public struct DropData : IData
{
    public short id;    // key
    /// <summary>
    /// 몬스터 테이블 참조
    /// </summary>
    public int monsterId;   // 몬스터 id
    public DropItemData item1;
    public DropItemData item2;
    public DropItemData item3;
    public bool isBoss;

    public void Clear()
    {
        id = 0;
        monsterId = 0;
        item1 = new DropItemData();
        item2 = new DropItemData();
        item3 = new DropItemData();
        isBoss = false;
    }
}

public struct DropItemData
{
    /// <summary>
    /// 아이템 테이블 참조
    /// </summary>
    public int itemId;  // 아이템 Id
    public float dropRate;    // 드랍 확률
    public short dropCount; // 드랍 갯수
    public DropItemData(int itemId, float dropRate, short dropCount)
    {
        this.itemId = itemId;
        this.dropRate = dropRate;
        this.dropCount = dropCount;
    }
}
