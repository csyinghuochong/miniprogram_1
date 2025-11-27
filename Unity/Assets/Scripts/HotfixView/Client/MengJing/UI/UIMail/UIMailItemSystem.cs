using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMailItem))]
    [FriendOf(typeof(UIMailItem))]
    public static partial class UIMailItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIMailItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.Text_State = rc.Get<GameObject>("Text_State").GetComponent<TMP_Text>();
            self.Text_Title = rc.Get<GameObject>("Text_Title").GetComponent<TMP_Text>();
            self.Text_Time = rc.Get<GameObject>("Text_Time").GetComponent<TMP_Text>();
            self.Text_DeleteTime = rc.Get<GameObject>("Text_DeleteTime").GetComponent<TMP_Text>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Button_OnClick = rc.Get<GameObject>("Button_OnClick").GetComponent<Button>();

            self.Button_OnClick.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIMailContent).Coroutine(); });
        }
        
    }
}