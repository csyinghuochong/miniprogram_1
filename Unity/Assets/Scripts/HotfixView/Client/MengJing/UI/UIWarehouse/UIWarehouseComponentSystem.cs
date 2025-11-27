using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIWarehouseComponent))]
    [FriendOf(typeof(UIWarehouseComponent))]
    public static partial class UIWarehouseComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIWarehouseComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Content_WarehouseItem = rc.Get<GameObject>("Content_WarehouseItem").transform;
            self.Content_BagItem = rc.Get<GameObject>("Content_BagItem").transform;

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIWarehouse); });
        }
        
        [EntitySystem]
        private static void Destroy(this UIWarehouseComponent self)
        {
            self.UIWarehouseItemList.Clear();
            self.UIBagItemList.Clear();
            self.UICommonItem = null;
        }
    }
}