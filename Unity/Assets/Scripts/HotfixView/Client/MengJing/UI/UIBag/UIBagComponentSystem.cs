using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class DataUpdate_UpdateUserData_UIBagRefresh : AEvent<Scene, UpdateUserData>
    {
        protected override async ETTask Run(Scene scene, UpdateUserData args)
        {
            if (args.UserDataType != UserDataType.Gold && args.UserDataType != UserDataType.Diamond)
            {
                return;
            }

            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIBag);
            if (ui == null)
            {
                return;
            }

            UIBagComponent uiBagComponent = ui.GetComponent<UIBagComponent>();
            if (args.UserDataType == UserDataType.Gold)
            {
                uiBagComponent.UpdateGold();
            }

            if (args.UserDataType == UserDataType.Diamond)
            {
                uiBagComponent.UpdateDiamond();
            }

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class InventoryUpdate_UIBagRefresh : AEvent<Scene, InventoryUpdate>
    {
        protected override async ETTask Run(Scene scene, InventoryUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIBag);
            if (ui == null)
            {
                return;
            }

            UIBagComponent uiBagComponent = ui.GetComponent<UIBagComponent>();
            uiBagComponent.UpdateItemList(uiBagComponent.CurrentPage);

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIBagComponent))]
    [FriendOf(typeof(UIBagComponent))]
    public static partial class UIBagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIBagComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
            self.Button_AddGold = rc.Get<GameObject>("Button_AddGold").GetComponent<Button>();
            self.Button_AddDiamond = rc.Get<GameObject>("Button_AddDiamond").GetComponent<Button>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Type_All = rc.Get<GameObject>("Button_Type_All").GetComponent<Button>();
            self.Button_Type_Consume = rc.Get<GameObject>("Button_Type_Consume").GetComponent<Button>();
            self.Button_Type_Equipment = rc.Get<GameObject>("Button_Type_Equipment").GetComponent<Button>();
            self.Button_Type_Material = rc.Get<GameObject>("Button_Type_Material").GetComponent<Button>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").GetComponent<Transform>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.Button_AddGold.onClick.AddListener(() => { Log.Warning("弹出来金币界面"); });
            self.Button_AddDiamond.onClick.AddListener(() => { Log.Warning("弹出来钻石界面"); });
            self.Button_Type_All.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_Consume.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_Equipment.onClick.AddListener(() => { self.SetShowType(2); });
            self.Button_Type_Material.onClick.AddListener(() => { self.SetShowType(3); });
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIBag); });

            self.UpdateGold();
            self.UpdateDiamond();
            self.SetShowType(0);
        }

        [EntitySystem]
        private static void Destroy(this UIBagComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItem = null;
        }

        private static void SetShowType(this UIBagComponent self, int page)
        {
            self.CurrentPage = page;
            self.Button_Type_All.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_All.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_Consume.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_Consume.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_Equipment.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_Equipment.transform.Find("Image_Off").gameObject.SetActive(page != 2);
            self.Button_Type_Material.transform.Find("Image_On").gameObject.SetActive(page == 3);
            self.Button_Type_Material.transform.Find("Image_Off").gameObject.SetActive(page != 3);

            self.UpdateItemList(page);
        }

        public static void UpdateItemList(this UIBagComponent self, int page)
        {
            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = null;
            if (page == 0)
            {
                itemList = inventoryComponentC.GetItemsByContainer(InventoryContainerType.Bag);
            }
            else if (page == 1)
            {
                itemList = inventoryComponentC.GetItemsByType(ItemType.Consume, InventoryContainerType.Bag);
            }
            else if (page == 2)
            {
                itemList = inventoryComponentC.GetItemsByType(ItemType.Equipment, InventoryContainerType.Bag);
            }
            else if (page == 3)
            {
                itemList = inventoryComponentC.GetItemsByType(ItemType.Material, InventoryContainerType.Bag);
            }
            else
            {
                return;
            }

            while (self.UICommonItemList.Count < (itemList.Count > 100 ? itemList.Count : 100))
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(itemList[i], (itemId) => { self.OnItemClick(itemId).Coroutine(); }).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);
                self.UICommonItemList[i].Item.SetActive(true);
            }

            for (int i = itemList.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].GameObject.SetActive(true);
                self.UICommonItemList[i].Image_ItemNull.gameObject.SetActive(true);
                self.UICommonItemList[i].Item.SetActive(false);
            }
        }

        private static async ETTask OnItemClick(this UIBagComponent self, long itemId)
        {
            UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
            if (uI != null)
            {
                uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                {
                    ItemId = itemId,
                    UIItemTipOpType = UIItemTipOpType.OnRoleBag
                });
            }
        }

        public static void UpdateGold(this UIBagComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();

            if (userInfoComponent.Gold >= 10000)
            {
                self.Text_Type_Gold.SetTextFormat("{0}k", userInfoComponent.Gold / 1000);
                return;
            }

            self.Text_Type_Gold.SetText(userInfoComponent.Gold);
        }

        public static void UpdateDiamond(this UIBagComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            
            if (userInfoComponent.Diamond >= 10000)
            {
                self.Text_Type_Diamond.SetTextFormat("{0}k", userInfoComponent.Diamond / 1000);
                return;
            }

            self.Text_Type_Diamond.SetText(userInfoComponent.Diamond);
        }
    }
}