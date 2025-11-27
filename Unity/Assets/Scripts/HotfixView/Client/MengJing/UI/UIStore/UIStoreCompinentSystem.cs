using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIStoreComponent))]
    [FriendOf(typeof(UIStoreComponent))]
    public static partial class UIStoreComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIStoreComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
            self.Button_AddGold = rc.Get<GameObject>("Button_AddGold").GetComponent<Button>();
            self.Button_AddDiamond = rc.Get<GameObject>("Button_AddDiamond").GetComponent<Button>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_RefreshTime = rc.Get<GameObject>("Text_RefreshTime").GetComponent<TMP_Text>();
            self.Button_RefreshTime = rc.Get<GameObject>("Button_Refresh").GetComponent<Button>();
            self.Content_UIStoreITem = rc.Get<GameObject>("Content_UIStoreItem").transform;
            self.UIStoreItem = rc.Get<GameObject>("UIStoreItem");
            self.UIStoreItem.SetActive(false);
            
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIStore); });
            
            self.UpdateStoreList();
        }
        
        [EntitySystem]
        private static void Destroy(this UIStoreComponent self)
        {
            self.UIStoreItemList.Clear();
            self.UIStoreItem = null;
        }

        private static void UpdateStoreList(this UIStoreComponent self)
        {
            
        }
    }
}