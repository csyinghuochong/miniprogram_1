using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class ChatUpdate_UIChatRefresh : AEvent<Scene, ChatUpdate>
    {
        protected override async ETTask Run(Scene scene, ChatUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIChat);
            if (ui == null)
            {
                return;
            }

            UIChatComponent uiChatComponent = ui.GetComponent<UIChatComponent>();
            uiChatComponent.UpdateItemList(uiChatComponent.CurrentPage);

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIChatComponent))]
    [FriendOf(typeof(UIChatComponent))]
    [FriendOf(typeof(UIPublicChatItem))]
    public static partial class UIChatComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIChatComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_Title = rc.Get<GameObject>("Text_Title").GetComponent<TMP_Text>();

            self.Scroll_PublicChatItem = rc.Get<GameObject>("Scroll_PublicChatItem");
            self.Content_UIPublicChatItem = rc.Get<GameObject>("Content_UIPublicChatItem").transform;
            self.UIPublicChatItem = rc.Get<GameObject>("UIPublicChatItem");
            self.UIPublicChatItem.SetActive(false);
            self.Scroll_PrivateChatPeopleItem = rc.Get<GameObject>("Scroll_PrivateChatPeopleItem");
            self.Content_UIPrivateChatPeopleItem = rc.Get<GameObject>("Content_UIPrivateChatPeopleItem").transform;
            self.UIPrivateChatPeopleItem = rc.Get<GameObject>("UIPrivateChatPeopleItem");
            self.UIPrivateChatPeopleItem.SetActive(false);
            self.Scroll_PrivateChatItem = rc.Get<GameObject>("Scroll_PrivateChatItem");
            self.Text_ChatPeopleName = rc.Get<GameObject>("Text_ChatPeopleName").GetComponent<TMP_Text>();
            self.Content_UIPrivateChatItem = rc.Get<GameObject>("Content_UIPrivateChatItem").transform;
            self.UIPrivateChatItem = rc.Get<GameObject>("UIPrivateChatItem");
            self.UIPrivateChatItem.SetActive(false);

            self.InputField_Content = rc.Get<GameObject>("InputField_Content").GetComponent<TMP_InputField>();
            self.Button_Emoji = rc.Get<GameObject>("Button_Emoji").GetComponent<Button>();
            self.Button_Send = rc.Get<GameObject>("Button_Send").GetComponent<Button>();
            self.Button_Type_World = rc.Get<GameObject>("Button_Type_World").GetComponent<Button>();
            self.Button_Type_LianMeng = rc.Get<GameObject>("Button_Type_LianMeng").GetComponent<Button>();
            self.Button_Type_PrivateChat = rc.Get<GameObject>("Button_Type_PrivateChat").GetComponent<Button>();
            self.GameObject_Emoji = rc.Get<GameObject>("GameObject_Emoji");
            self.Button_CloseEmoji = rc.Get<GameObject>("Button_CloseEmoji").GetComponent<Button>();
            self.Content_EmojiList = rc.Get<GameObject>("Content_EmojiList");

            self.GameObject_Emoji.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIChat); });
            self.Button_Emoji.AddListener(() => { self.GameObject_Emoji.SetActive(true); });
            self.Button_Type_World.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_LianMeng.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_PrivateChat.AddListener(() => { self.SetShowType(2); });
            self.Button_CloseEmoji.AddListener(() => { self.GameObject_Emoji.SetActive(false); });
            self.Button_Send.AddListener(() => { self.OnButton_Send().Coroutine(); });
            self.InputField_Content.onValueChanged.AddListener((string s) =>
            {
                string text_new = "";
                self.Root().GetComponent<MaskWordComponent>().IsContainSensitiveWords(ref s, out text_new);
                self.InputField_Content.SetTextWithoutNotify(s);
            });

            for (int i = 0; i < self.Content_EmojiList.transform.childCount; i++)
            {
                GameObject go = self.Content_EmojiList.transform.GetChild(i).gameObject;
                go.GetComponent<Button>().AddListener(() => { self.InputField_Content.text += $"<sprite={go.name}>"; });
            }
        }

        public static void SetShowType(this UIChatComponent self, int page, FriendData friendData = null)
        {
            self.CurrentPage = page;
            self.FriendData = friendData;
            self.Button_Type_World.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_World.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_LianMeng.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_LianMeng.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_PrivateChat.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_PrivateChat.transform.Find("Image_Off").gameObject.SetActive(page != 2);

            self.Scroll_PublicChatItem.gameObject.SetActive(page == 0);
            self.Scroll_PrivateChatPeopleItem.gameObject.SetActive(page == 1);
            self.Scroll_PrivateChatItem.gameObject.SetActive(page == 2);

            self.UpdateItemList(page);
        }

        public static void UpdateItemList(this UIChatComponent self, int page)
        {
            ChatComponentC chatComponent = self.Root().GetComponent<ChatComponentC>();
            List<Chat> chatList = null;
            if (page == 0)
            {
                chatList = chatComponent.GetWorldChatList();

                while (self.UIChatItemList.Count < chatList.Count)
                {
                    GameObject go = UnityEngine.Object.Instantiate(self.UIPublicChatItem, self.Content_UIPublicChatItem);
                    UIPublicChatItem newItem = self.AddChild<UIPublicChatItem, GameObject>(go);
                    self.UIChatItemList.Add(newItem);
                }

                for (int i = 0; i < chatList.Count; i++)
                {
                    self.UIChatItemList[i].UpdateInfo(chatList[i]);
                    self.UIChatItemList[i].GameObject.SetActive(true);
                }

                for (int i = chatList.Count; i < self.UIChatItemList.Count; i++)
                {
                    self.UIChatItemList[i].GameObject.SetActive(false);
                }

                // 移动到底部
                Canvas.ForceUpdateCanvases();
                self.Content_UIPublicChatItem.parent.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
                Canvas.ForceUpdateCanvases();
            }
            else if (page == 1)
            {
                chatList = chatComponent.GetAllianceChatList();
            }
            else if (page == 2)
            {
                if (self.FriendData != null)
                {
                    self.Text_ChatPeopleName.SetText(self.FriendData.PlayerName);

                    chatList = chatComponent.GetFriendChatList(self.FriendData.UnitId);

                    while (self.UIPrivateChatItemList.Count < chatList.Count)
                    {
                        GameObject go = UnityEngine.Object.Instantiate(self.UIPrivateChatItem, self.Content_UIPrivateChatItem);
                        UIPrivateChatItem newItem = self.AddChild<UIPrivateChatItem, GameObject>(go);
                        self.UIPrivateChatItemList.Add(newItem);
                    }

                    for (int i = 0; i < chatList.Count; i++)
                    {
                        self.UIPrivateChatItemList[i].UpdateInfo(chatList[i]);
                        self.UIPrivateChatItemList[i].GameObject.SetActive(true);
                    }

                    for (int i = chatList.Count; i < self.UIPrivateChatItemList.Count; i++)
                    {
                        self.UIPrivateChatItemList[i].GameObject.SetActive(false);
                    }

                    // 移动到底部
                    Canvas.ForceUpdateCanvases();
                    self.Content_UIPrivateChatItem.parent.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
                    Canvas.ForceUpdateCanvases();
                }
            }
        }

        private static async ETTask OnButton_Send(this UIChatComponent self)
        {
            string input = self.InputField_Content.text;

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (input.Length > ConfigData.ChatContentMax)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("内容太长");
                return;
            }

            if (TimeHelper.ServerNow() - self.LastSendTime < ConfigData.ChatInterval)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("发送过于频繁");
                return;
            }

            self.InputField_Content.SetTextWithoutNotify("");
            self.LastSendTime = TimeHelper.ServerNow();

            int error = 0;
            switch (self.CurrentPage)
            {
                case 0:
                    error = await ClientChatHelper.SendChat(self.Root(), input, ChatRoomType.World);
                    break;
                case 1:
                    error = await ClientChatHelper.SendChat(self.Root(), input, ChatRoomType.Alliance);
                    break;
                case 2:
                    error = await ClientChatHelper.SendChat(self.Root(), input, ChatRoomType.Private, self.FriendData.UnitId);
                    break;
            }

            if (error != ErrorCode.ERR_Success)
            {
                return;
            }
        }
    }
}