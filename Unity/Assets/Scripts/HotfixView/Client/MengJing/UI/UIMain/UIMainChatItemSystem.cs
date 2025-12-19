
using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainChatItem))]
    [FriendOf(typeof(UIMainChatItem))]
    public static partial class UIMainChatItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainChatItem self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_ChatType = rc.Get<GameObject>("Text_ChatType").GetComponent<TMP_Text>();
            self.Text_ChatContent = rc.Get<GameObject>("Text_ChatContent").GetComponent<TMP_Text>();
        }
        
    }
}