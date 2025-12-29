using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIPrivateChatPeopleItem))]
    [FriendOf(typeof(UIPrivateChatPeopleItem))]
    public static partial class UIPrivateChatPeopleItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIPrivateChatPeopleItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_SpeakerHead = rc.Get<GameObject>("Image_SpeakerHead").GetComponent<Image>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Button_EnterChat = rc.Get<GameObject>("Button_EnterChat").GetComponent<Button>();

            self.Button_EnterChat.onClick.AddListener(() => self.OnButton_EnterChat());
        }

        public static void UpdateInfo(this UIPrivateChatPeopleItem self, FriendData friendData)
        {
            self.FriendData = friendData;
            self.Text_PlayerName.text = friendData.PlayerName;
        }

        private static void OnButton_EnterChat(this UIPrivateChatPeopleItem self)
        {
            self.GetParent<UIChatComponent>().ShowPrivateChat(self.FriendData);
        }
    }
}