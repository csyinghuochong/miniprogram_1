using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTip_ConsumeComponent))]
    [FriendOf(typeof(UIItemTip_ConsumeComponent))]
    public static partial class UIItemTip_ConsumeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTip_ConsumeComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_ItemDescription = rc.Get<GameObject>("Text_ItemDescription").GetComponent<TMP_Text>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();
            self.Button_Use = rc.Get<GameObject>("Button_Use").GetComponent<Button>();

            self.Button_Sell.onClick.AddListener(() => { self.OnButton_Sell(); });
            self.Button_Use.onClick.AddListener(() => { Log.Warning("使用道具"); });
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_ConsumeComponent self)
        {
        }

        private static void OnButton_Sell(this UIItemTip_ConsumeComponent self)
        {
            self.Root().GetComponent<UIComponent>().Create(UIType.UIItemSellTip).Coroutine();
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }
        
        public static void UpdateInfo(this UIItemTip_ConsumeComponent self, UIItemTipData uiItemTipData)
        {
            self.UIItemTipData = uiItemTipData;

            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(uiItemTipData.ItemId);
            if (item != null)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
                self.Text_ItemName.text = itemConfig.ItemName;
                self.Text_ItemDescription.text = itemConfig.ItemDescription;
            }
        }
    }
}