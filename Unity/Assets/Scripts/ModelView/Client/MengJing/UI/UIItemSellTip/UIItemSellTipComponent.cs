using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIItemSellTipComponent : Entity, IAwake, IDestroy
    {
        public UIItemTipData UIItemTipData;
        public int ItemMaxNum;
        
        public TMP_InputField InputField_SellNum;
        public TMP_Text Text_SellPrice;
        
        public Button Button_Less;
        public Button Button_Add;
        public Button Button_Cancel;
        public Button Button_Sell;
    }
}