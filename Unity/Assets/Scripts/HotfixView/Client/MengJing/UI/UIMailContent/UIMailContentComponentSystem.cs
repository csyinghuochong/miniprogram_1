using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMailContentComponent))]
    [FriendOf(typeof(UIMailContentComponent))]
    public static partial class UIMailContentComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMailContentComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Title = rc.Get<GameObject>("Text_Title").GetComponent<TMP_Text>();
            self.Text_From = rc.Get<GameObject>("Text_From").GetComponent<TMP_Text>();
            self.Text_Time = rc.Get<GameObject>("Text_Time").GetComponent<TMP_Text>();
            self.Text_Content = rc.Get<GameObject>("Text_Content").GetComponent<TMP_Text>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Get = rc.Get<GameObject>("Button_Get").GetComponent<Button>();
            self.Button_Delete = rc.Get<GameObject>("Button_Delete").GetComponent<Button>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIMail); });
            self.Button_Get.onClick.AddListener(() => { self.OnGet(); });
            self.Button_Delete.onClick.AddListener(() => { self.OnDelete(); });
        }
        
        [EntitySystem]
        private static void Destroy(this UIMailContentComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItem = null;
        }

        private static void OnDelete(this UIMailContentComponent self)
        {
            Log.Warning("删除了一个邮件");
        }

        private static void OnGet(this UIMailContentComponent self)
        {
            Log.Warning("领取了一个邮件");
        }
    }
}