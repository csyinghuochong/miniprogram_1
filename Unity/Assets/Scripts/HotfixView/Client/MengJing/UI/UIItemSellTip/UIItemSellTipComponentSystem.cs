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

            self.Button_Cancel.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemSellTip); });


        }

        [EntitySystem]
        private static void Destroy(this UIItemSellTipComponent self)
        {
        }
        
        
    }
}