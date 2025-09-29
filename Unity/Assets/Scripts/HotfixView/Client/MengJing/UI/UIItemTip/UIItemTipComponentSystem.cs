using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTipComponent))]
    [FriendOf(typeof(UIItemTipComponent))]
    public static partial class UIItemTipComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTipComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_ItemDescription = rc.Get<GameObject>("Text_ItemDescription").GetComponent<TMP_Text>();
            self.Button_Use = rc.Get<GameObject>("Button_Use").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip); });
            self.Button_Use.onClick.AddListener(() => { Log.Warning("使用道具"); });
        }

        [EntitySystem]
        private static void Destroy(this UIItemTipComponent self)
        {
        }

        public static void UpdateInfo(this UIItemTipComponent self, long itemId)
        {
            self.ItemId = itemId;

            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(itemId);
            if (item != null)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
                self.Text_ItemName.text = itemConfig.ItemName;
                self.Text_ItemDescription.text = itemConfig.ItemDescription;
            }
        }
    }
}