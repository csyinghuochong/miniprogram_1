using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIChatItem))]
    [FriendOf(typeof(UIChatItem))]
    public static partial class UIChatItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIChatItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_SpeakerHead = rc.Get<GameObject>("Image_SpeakerHead").GetComponent<Image>();
            self.Button_OnSpeakerHead = rc.Get<GameObject>("Button_OnSpeakerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_Content = rc.Get<GameObject>("Text_Content").GetComponent<TMP_Text>();

            self.Button_OnSpeakerHead.AddListener(() => { self.OnButton_OnSpeakerHead().Coroutine(); });
        }

        public static void UpdateInfo(this UIChatItem self, ChatEntry chatEntry)
        {
            self.ChatEntry = chatEntry;

            self.Text_PlayerName.SetText(chatEntry.Name);
            self.Text_Content.SetText(chatEntry.Content);
        }

        private static async ETTask OnButton_OnSpeakerHead(this UIChatItem self)
        {
            M2C_WatchPlayer response = await ClientUserInfoHelper.WatchPlayer(self.Root(), self.ChatEntry.UnitId);

            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIPlayerInfo);
            UIPlayerInfoComponent uiPlayerInfoComponent = ui.GetComponent<UIPlayerInfoComponent>();
            uiPlayerInfoComponent.UpdateInfo(response);
        }
    }
}