using System.Collections.Generic;
using System.Linq;
using QQ;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectQQ.Scripts.UI.Popup
{
    public class UIClearReward : UIPopup<UIClearReward>
    {
        [SerializeField] private Transform slotParent;
        [SerializeField] private UIButtonAndText btnExit;

        private Actor actor;
        private List<UIButtonAndText> btnSlots = new List<UIButtonAndText>();
        private Dictionary<UIButtonAndText, ItemData> btnToData = new Dictionary<UIButtonAndText, ItemData>();

        private const int slotCount = 3;

        protected override void OnInit()
        {
            actor = GameManager.Instance.Player; // Actor 참조

            var rewardCandidates = ItemDataManager.Instance.GetRandomRouletteItems();

            foreach (var itemData in rewardCandidates)
            {
                GameObject itemSlotPrefab = ResManager.LoadResource<GameObject>(ResType.UI, "UIItemSlot");
                if (itemSlotPrefab == null)
                {
                    Debug.LogError("UIItemSlot 프리팹을 찾을 수 없습니다.");
                    continue;
                }

                GameObject itemSlotGO = Instantiate(itemSlotPrefab, slotParent);
                UIItemSlot itemSlot = itemSlotGO.GetOrAddComponent<UIItemSlot>();
                itemSlot.SetData(itemData);

                UIButtonAndText button = itemSlotGO.GetComponent<UIButtonAndText>();
                if (button != null)
                {
                    btnSlots.Add(button);
                    btnToData[button] = itemData;
                }
                else
                {
                    Debug.LogWarning("UIButtonAndText 컴포넌트를 찾을 수 없습니다.");
                }
            }

            // 이벤트 초기화 (클리어)
            btnExit.OnClickClear();
            foreach (var button in btnSlots)
            {
                button.OnClickClear();
            }
        }
        
        protected override void OnStart()
        {
            btnExit.OnClickAdd(OnClickExit);
            btnExit.SetText("X");

            foreach (var button in btnSlots)
            {
                if (btnToData.TryGetValue(button, out var itemData))
                {
                    button.OnClickAdd(() => OnClickItemSlot(itemData));
                }
            }
        }

        protected override void OnFocus()
        {
        }

        protected override void OnLostFocus()
        {
        }

        protected override void OnExit()
        {
            btnExit.OnClickRemove(OnClickExit);

            foreach (var button in btnSlots)
            {
                if (btnToData.TryGetValue(button, out var itemData))
                {
                    button.OnClickRemove(() => OnClickItemSlot(itemData));
                }
            }

            btnSlots.Clear();
            btnToData.Clear();
        }

        private void OnClickItemSlot(ItemData selectedItem)
        {
            // 선택한 아이템 인벤토리에 넣기
            
            OnClickExit();
        }

        private void OnClickExit()
        {
            Debug.Log("OnClickExit");
            CloseUI();
        }
    }
}
