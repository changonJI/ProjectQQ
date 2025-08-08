using QQ;

public struct ItemData : IData
{
    public short id;                 // 아이템 고유 ID
    public ItemType itemType;       // 0=상점, 1=소비, 2=공격, 3=패시브, 4=상자, 5=조합
    public int nameId;              // 이름 텍스트 ID (언어 테이블)
    public int desId;               // 설명 텍스트 ID (언어 테이블)
    public string iconName;         // UI 아이콘 파일명 (png)
    
    public short lvCount;           // 아이템 레벨 수 (최대 성장 단계)
    public int skillId;             // 연결된 스킬 ID
    public short targetType;        // 타겟팅
    
    public int nextID;         // 진화 대상 아이템 ID

    public bool isShopItem;         // 상점 노출 여부

    public void Clear()
    {
        id = 0;
        itemType = ItemType.None;
        nameId = 0;
        desId = 0;
        iconName = string.Empty;
        lvCount = 0;
        skillId = 0;
        targetType = 0;
        nextID = 0;
        isShopItem = false;
    }

    public void Set(ItemData data)
    {
        id = data.id;
        itemType = data.itemType;
        nameId = data.nameId;
        desId = data.desId;
        iconName = data.iconName;
        lvCount = data.lvCount;
        skillId = data.skillId;
        targetType = data.targetType;
        nextID = data.nextID;
        isShopItem = data.isShopItem;
    }
}
