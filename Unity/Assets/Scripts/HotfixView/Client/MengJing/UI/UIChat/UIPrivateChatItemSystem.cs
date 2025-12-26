using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIPrivateChatItem))]
    [FriendOf(typeof(UIPrivateChatItem))]
    public static partial class UIPrivateChatItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIPrivateChatItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_SpeakerHead = rc.Get<GameObject>("Image_SpeakerHead").GetComponent<Image>();
            self.Button_OnSpeakerHead = rc.Get<GameObject>("Button_OnSpeakerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_Content = rc.Get<GameObject>("Text_Content").GetComponent<TMP_Text>();

            self.Button_OnSpeakerHead.AddListener(() => { self.OnButton_OnSpeakerHead().Coroutine(); });
        }

        public static void UpdateInfo(this UIPrivateChatItem self, Chat chat)
        {
            self.Chat = chat;

            self.Text_PlayerName.SetText(chat.Name);
            self.Text_Content.SetText(chat.Content);
        }

        private static async ETTask OnButton_OnSpeakerHead(this UIPrivateChatItem self)
        {
            M2C_WatchPlayer response = await ClientUserInfoHelper.WatchPlayer(self.Root(), self.Chat.UnitId);

            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIPlayerInfo);
            UIPlayerInfoComponent uiPlayerInfoComponent = ui.GetComponent<UIPlayerInfoComponent>();
            uiPlayerInfoComponent.UpdateInfo(response.WatchPlayerInfo);
        }
    }
}