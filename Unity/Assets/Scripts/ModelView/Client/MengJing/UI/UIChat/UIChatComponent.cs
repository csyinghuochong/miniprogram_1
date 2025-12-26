using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIChatComponent: Entity, IAwake
    {
        private EntityRef<FriendData> friendData;
        public FriendData FriendData { get => this.friendData; set => this.friendData = value; }

        public int CurrentPage { get; set; } = 0;
        public List<UIPublicChatItem> UIChatItemList { get; set; } = new();
        public List<UIPrivateChatPeopleItem> UIPrivateChatPeopleItemList { get; set; } = new();
        public List<UIPrivateChatItem> UIPrivateChatItemList { get; set; } = new();
        
        public Button Button_Close;
        public TMP_Text Text_Title;
        
        public GameObject Scroll_PublicChatItem;
        public Transform Content_UIPublicChatItem;
        public GameObject UIPublicChatItem;
        public GameObject Scroll_PrivateChatPeopleItem;
        public Transform Content_UIPrivateChatPeopleItem;
        public GameObject UIPrivateChatPeopleItem;
        public GameObject Scroll_PrivateChatItem;
        public TMP_Text Text_ChatPeopleName;
        public Transform Content_UIPrivateChatItem;
        public GameObject UIPrivateChatItem;
        
        public TMP_InputField InputField_Content;
        public Button Button_Emoji;
        public Button Button_Send;
        public Button Button_Type_World;
        public Button Button_Type_LianMeng;
        public Button Button_Type_PrivateChat;
        public GameObject GameObject_Emoji;
        public Button Button_CloseEmoji;
        public GameObject Content_EmojiList;
    }
}