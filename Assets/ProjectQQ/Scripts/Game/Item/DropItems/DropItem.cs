using QQ;
using UnityEngine;

public class DropItem : Item
{
    [SerializeField] private SpriteRenderer iconImage; // UI 이미지 (아이템 아이콘 표시용)

    private ItemData _itemData;

    protected override void OnInit()
    {
        base.OnInit();
    }

    /// <summary>
    /// 드랍 아이템 초기화
    /// </summary>
    public void DataInit(ItemData itemData)
    {
        _itemData = itemData;

        iconImage.sprite = LoadItemIcon(_itemData.iconName);
        // 이펙트 적용
    }

    /// <summary>
    /// 플레이어가 이 아이템에 닿았을 때 호출
    /// </summary>
    public void OnCollected()
    {
        // 인벤토리 등에 아이템 추가
        // PoolManager.Instance.actor.GetInventory(

        // 드랍 아이템 제거 (PoolManager 사용 시 반환)
        PoolManager.Instance.ReleaseObject(gameObject);
    }

    /// <summary>
    /// 아이콘 리소스 불러오기
    /// </summary>
    private Sprite LoadItemIcon(string iconName)
    {
        return ResManager.LoadResource<Sprite>(ResType.Sprite, $"Items/{iconName}");
    }

    public ItemData GetItemData()
    {
        return _itemData;
    }
}
