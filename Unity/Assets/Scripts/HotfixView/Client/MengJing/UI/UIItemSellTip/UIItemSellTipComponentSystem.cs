using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemSellTipComponent))]
    [FriendOf(typeof(UIItemSellTipComponent))]
    public static partial class UIItemSellTipComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemSellTipComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.InputField_SellNum = rc.Get<GameObject>("InputField_SellNum").GetComponent<TMP_InputField>();
            self.Text_SellPrice = rc.Get<GameObject>("Text_SellPrice").GetComponent<TMP_Text>();
            self.Button_Less = rc.Get<GameObject>("Button_Less").GetComponent<Button>();
            self.Button_Add = rc.Get<GameObject>("Button_Add").GetComponent<Button>();
            self.Button_Cancel = rc.Get<GameObject>("Button_Cancel").GetComponent<Button>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();

            
            self.Button_Less.AddListener(self.OnButton_Less);
            self.Button_Add.AddListener(self.OnButton_Add);
            self.Button_Cancel.AddListener(self.OnButton_Cancel);
            self.Button_Sell.AddListener(self.OnButton_Sell);
        }

        [EntitySystem]
        private static void Destroy(this UIItemSellTipComponent self)
        {
        }

        public static void OnButton_Less(this UIItemSellTipComponent self)
        {
            int sellNum = 0;
            sellNum = int.Parse(self.InputField_SellNum.text);

            sellNum--;

            if (sellNum <= 0)
            {
                return;
            }

            self.InputField_SellNum.text = sellNum.ToString();
            self.UpdateSellPrice();
        }

        public static void OnButton_Add(this UIItemSellTipComponent self)
        {
            int sellNum = 0;
            sellNum = int.Parse(self.InputField_SellNum.text);

            sellNum++;

            if (sellNum > self.ItemMaxNum)
            {
                return;
            }

            self.InputField_SellNum.text = sellNum.ToString();
            self.UpdateSellPrice();
        }

        public static void OnButton_Cancel(this UIItemSellTipComponent self)
        {
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemSellTip);
        }

        private static void OnButton_Sell(this UIItemSellTipComponent self)
        {
            int sellNum = 0;
            sellNum = int.Parse(self.InputField_SellNum.text);

            if (sellNum <= 0 || sellNum > self.ItemMaxNum)
            {
                return;
            }
            
            ClientInventoryHelper.SellItem(self.Root(), self.UIItemTipData.Item.Id, sellNum).Coroutine();
            self.OnButton_Cancel();
        }

        public static void UpdateSellPrice(this UIItemSellTipComponent self)
        {
            int sellNum = 0;
            sellNum = int.Parse(self.InputField_SellNum.text);

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.UIItemTipData.Item.ConfigId);

            long price = sellNum * itemConfig.SellMoneyValue;
            self.Text_SellPrice.text = price.ToString();
        }

        public static void InitUI(this UIItemSellTipComponent self,UIItemTipData uiItemTipData)
        {
            self.UIItemTipData = uiItemTipData;
            self.ItemMaxNum = self.UIItemTipData.Item.Num;

            self.InputField_SellNum.text = self.ItemMaxNum.ToString();
            self.UpdateSellPrice();
        }
    }
}