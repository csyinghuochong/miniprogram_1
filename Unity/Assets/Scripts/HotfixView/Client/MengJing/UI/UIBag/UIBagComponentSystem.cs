using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIBagComponent))]
    [FriendOf(typeof(UIBagComponent))]
    public static partial class UIBagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIBagComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Type_All = rc.Get<GameObject>("Button_Type_All").GetComponent<Button>();
            self.Button_Type_Material = rc.Get<GameObject>("Button_Type_Material").GetComponent<Button>();
            self.Button_Type_Consume = rc.Get<GameObject>("Button_Type_Consume").GetComponent<Button>();
            self.Button_Type_HeroShard = rc.Get<GameObject>("Button_Type_HeroShard").GetComponent<Button>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").GetComponent<Transform>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.UICommonItem.gameObject.SetActive(false);

            self.Button_Type_All.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_Material.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_Consume.onClick.AddListener(() => { self.SetShowType(2); });
            self.Button_Type_HeroShard.onClick.AddListener(() => { self.SetShowType(3); });
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIBag); });

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
            self.Button_Type_All.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_All.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_Material.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_Material.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_Consume.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_Consume.transform.Find("Image_Off").gameObject.SetActive(page != 2);
            self.Button_Type_HeroShard.transform.Find("Image_On").gameObject.SetActive(page == 3);
            self.Button_Type_HeroShard.transform.Find("Image_Off").gameObject.SetActive(page != 3);

            self.UpdateItemList(page);
        }

        private static void UpdateItemList(this UIBagComponent self, int page)
        {
            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = null;
            if (page == 0)
            {
                itemList = inventoryComponentC.GetAllItems();
            }
            else if (page == 1)
            {
                itemList = inventoryComponentC.GetItemsByType(ItemType.Material);
            }
            else if (page == 2)
            {
                itemList = inventoryComponentC.GetItemsByType(ItemType.Consume);
            }
            else if (page == 3)
            {
                itemList = inventoryComponentC.GetItemsByType(ItemType.HeroShard);
            }
            else
            {
                return;
            }

            while (self.UICommonItemList.Count < itemList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(itemList[i]);
                self.UICommonItemList[i].GameObject.SetActive(true);
            }

            for (int i = itemList.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].GameObject.SetActive(false);
            }
        }
    }
}