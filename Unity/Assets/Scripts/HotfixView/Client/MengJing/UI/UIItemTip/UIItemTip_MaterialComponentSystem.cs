using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTip_MaterialComponent))]
    [FriendOf(typeof(UIItemTip_MaterialComponent))]
    public static partial class UIItemTip_MaterialComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTip_MaterialComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_ItemDescription = rc.Get<GameObject>("Text_ItemDescription").GetComponent<TMP_Text>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();

            self.Button_Sell.onClick.AddListener(() => { self.OnButton_Sell(); });
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_MaterialComponent self)
        {
        }
        
        private static void OnButton_Sell(this UIItemTip_MaterialComponent self)
        {
            self.Root().GetComponent<UIComponent>().Create(UIType.UIItemSellTip).Coroutine();
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }
        
        public static void UpdateInfo(this UIItemTip_MaterialComponent self, UIItemTipData uiItemTipData)
        {
            self.UIItemTipData =  uiItemTipData;

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