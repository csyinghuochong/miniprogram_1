using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class InventoryUpdate_UIWarehouseRefresh : AEvent<Scene, InventoryUpdate>
    {
        protected override async ETTask Run(Scene scene, InventoryUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIWarehouse);
            if (ui == null)
            {
                return;
            }

            UIWarehouseComponent uiWarehouseComponent = ui.GetComponent<UIWarehouseComponent>();
            uiWarehouseComponent.UpdateWarehouseItemList();
            uiWarehouseComponent.UpdateBagItemList();

            await ETTask.CompletedTask;
        }
    }

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

            self.UpdateWarehouseItemList();
            self.UpdateBagItemList();
        }

        [EntitySystem]
        private static void Destroy(this UIWarehouseComponent self)
        {
            self.UIWarehouseItemList.Clear();
            self.UIBagItemList.Clear();
            self.UICommonItem = null;
        }

        public static void UpdateWarehouseItemList(this UIWarehouseComponent self)
        {
            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = null;
            itemList = inventoryComponentC.GetItemsByContainer(InventoryContainerType.Warehouse);

            while (self.UIWarehouseItemList.Count < (itemList.Count > 100 ? itemList.Count : 100))
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_WarehouseItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UIWarehouseItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UIWarehouseItemList[i].UpdateInfo(itemList[i], (itemId) => { self.OnWarehouseItemClick(itemId).Coroutine(); }).Coroutine();
                self.UIWarehouseItemList[i].GameObject.SetActive(true);
                self.UIWarehouseItemList[i].Item.SetActive(true);
            }

            for (int i = itemList.Count; i < self.UIWarehouseItemList.Count; i++)
            {
                self.UIWarehouseItemList[i].GameObject.SetActive(true);
                self.UIWarehouseItemList[i].Image_ItemNull.gameObject.SetActive(true);
                self.UIWarehouseItemList[i].Item.SetActive(false);
            }
        }

        public static void UpdateBagItemList(this UIWarehouseComponent self)
        {
            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = null;
            itemList = inventoryComponentC.GetItemsByContainer(InventoryContainerType.Bag);

            while (self.UIBagItemList.Count < (itemList.Count > 100 ? itemList.Count : 100))
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_BagItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UIBagItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UIBagItemList[i].UpdateInfo(itemList[i], (itemId) => { self.OnBagItemClick(itemId).Coroutine(); }).Coroutine();
                self.UIBagItemList[i].GameObject.SetActive(true);
                self.UIBagItemList[i].Item.SetActive(true);
            }

            for (int i = itemList.Count; i < self.UIBagItemList.Count; i++)
            {
                self.UIBagItemList[i].GameObject.SetActive(true);
                self.UIBagItemList[i].Image_ItemNull.gameObject.SetActive(true);
                self.UIBagItemList[i].Item.SetActive(false);
            }
        }

        private static async ETTask OnWarehouseItemClick(this UIWarehouseComponent self, long itemId)
        {
            UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
            if (uI != null)
            {
                uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                {
                    ItemId = itemId,
                    UIItemTipOpType = UIItemTipOpType.Warehouse2Bag
                });
            }
        }

        private static async ETTask OnBagItemClick(this UIWarehouseComponent self, long itemId)
        {
            UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
            if (uI != null)
            {
                uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                {
                    ItemId = itemId,
                    UIItemTipOpType = UIItemTipOpType.Bag2Warehouse
                });
            }
        }
    }
}