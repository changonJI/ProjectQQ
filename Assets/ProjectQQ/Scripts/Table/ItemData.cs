using QQ;

public struct ItemData : IData
{
    public short id;    // key
    /// <summary>
    /// BoxItemType은 상자 테이블의 group_id를 참조한다.
    /// </summary>
    public ItemType itemType;   // 아이템타입. 
    /// <summary>
    /// 언어 테이블 참조
    /// </summary>
    public int nameId;  // 아이템 이름
    /// <summary>
    /// 언어 테이블 참조
    /// </summary>
    public int desId;    // 아이템 설명
    public string iconName; // 아이템 이미지 리소스명
    public short lvCount; // 현재 보유 수량. 중복으로 먹었을시 더 큰 값으로 가져올때 사용
    /// <summary>
    /// 스킬 테이블 참조
    /// </summary>
    public int skillId; // skillID값
    public int boxValue; // BoxItemType에 따라 상자에서 나오는 아이템의 ID값

    public void Clear()
    {
        id = 0;
        itemType = ItemType.None;
        nameId = 0;
        desId = 0;
        iconName = string.Empty;
        lvCount = 0;
        skillId = 0;
        boxValue = 0;
    }
}
