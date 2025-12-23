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

            self.Button_OnSpeakerHead.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIPlayerInfo).Coroutine(); });
        }

        public static void UpdateInfo(this UIChatItem self, ChatEntry chatEntry)
        {
            self.ChatEntry = chatEntry;

            self.Text_PlayerName.SetText(chatEntry.Name);
            self.Text_Content.SetText(chatEntry.Content);
        }
    }
}