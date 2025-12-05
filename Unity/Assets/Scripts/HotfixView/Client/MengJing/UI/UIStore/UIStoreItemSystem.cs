using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIStoreItem))]
    [FriendOf(typeof(UIStoreItem))]
    public static partial class UIStoreItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIStoreItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_Num = rc.Get<GameObject>("Text_Num").GetComponent<TMP_Text>();
            self.Button_Buy = rc.Get<GameObject>("Button_Buy").GetComponent<Button>();
            self.Image_MoneyIcon = rc.Get<GameObject>("Image_MoneyIcon").GetComponent<Image>();
            self.Text_MoneyValue = rc.Get<GameObject>("Text_MoneyValue").GetComponent<TMP_Text>();
            self.Transform_Item = rc.Get<GameObject>("Transform_Item").transform;

            self.UICommonItem = self.AddChild<UICommonItem, GameObject>(rc.Get<GameObject>("UICommonItem"));
        }

        public static void UpdateInfo(this UIStoreItem self, int id, int num)
        {
            StoreItemConfig storeItemConfig = StoreItemConfigCategory.Instance.Get(id);
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(storeItemConfig.SellItemID);

            self.Text_ItemName.SetText(itemConfig.ItemName);
            self.Text_Num.SetText("剩余数量：{0}", num);
            self.Text_MoneyValue.SetText(storeItemConfig.SellValue);

            self.UICommonItem.UpdateInfo(itemConfig.Id, 0).Coroutine();
        }
    }
}