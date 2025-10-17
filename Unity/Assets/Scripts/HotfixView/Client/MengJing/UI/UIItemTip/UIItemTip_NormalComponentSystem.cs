using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTip_NormalComponent))]
    [FriendOf(typeof(UIItemTip_NormalComponent))]
    public static partial class UIItemTip_NormalComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTip_NormalComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_ItemDescription = rc.Get<GameObject>("Text_ItemDescription").GetComponent<TMP_Text>();
            self.Button_Use = rc.Get<GameObject>("Button_Use").GetComponent<Button>();

            self.Button_Use.onClick.AddListener(() => { Log.Warning("使用道具"); });
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_NormalComponent self)
        {
        }
        
        public static void UpdateInfo(this UIItemTip_NormalComponent self, long itemId)
        {
            self.ItemId = itemId;

            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(itemId);
            if (item != null)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
                self.Text_ItemName.text = itemConfig.ItemName;
                self.Text_ItemDescription.text = itemConfig.ItemDescription;
                self.Button_Use.gameObject.SetActive(itemConfig.ItemType == (int)ItemType.Consume);
            }
        }
    }
}