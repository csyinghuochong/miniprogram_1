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
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
        }
        
    }
}