using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMailComponent))]
    [FriendOf(typeof(UIMailComponent))]
    public static partial class UIMailComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMailComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_DeleteAll = rc.Get<GameObject>("Button_DeleteAll").GetComponent<Button>();
            self.Button_GetAll = rc.Get<GameObject>("Button_GetAll").GetComponent<Button>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIMailItem = rc.Get<GameObject>("Content_UIMailItem").transform;
            self.UIMailItem = rc.Get<GameObject>("UIMailItem");

            self.Button_DeleteAll.onClick.AddListener(() => { self.OnDeleteAll(); });
            self.Button_GetAll.onClick.AddListener(() => { self.OnGetAll(); });
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIMail); });
        }

        [EntitySystem]
        private static void Destroy(this UIMailComponent self)
        {
        }

        private static void OnDeleteAll(this UIMailComponent self)
        {
            Log.Warning("删除了所有邮件");
        }

        private static void OnGetAll(this UIMailComponent self)
        {
            Log.Warning("领取了所有邮件");
        }
    }
}