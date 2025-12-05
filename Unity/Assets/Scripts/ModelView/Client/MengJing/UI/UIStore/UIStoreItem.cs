using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIStoreItem : Entity, IAwake<GameObject>
    {
        public int StoreItemConfigId;
        public int Num;
        public Action<int> BuyAction;
        
        public GameObject GameObject { get; set; }

        public TMP_Text Text_ItemName;
        public TMP_Text Text_Num;
        public Button Button_Buy;
        public Image Image_MoneyIcon;
        public TMP_Text Text_MoneyValue;
        public Transform Transform_Item;
        public UICommonItem UICommonItem { get; set; }
    }
}