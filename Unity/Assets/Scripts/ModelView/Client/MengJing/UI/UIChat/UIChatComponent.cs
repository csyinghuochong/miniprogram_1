using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIChatComponent: Entity, IAwake
    {
        public int CurrentPage { get; set; } = 0;
        public List<UIChatItem> UIChatItemList { get; set; } = new();
        
        public Button Button_Close;
        public TMP_Text Text_Title;
        public Transform Content_UIPublicChatItem;
        public GameObject UIPublicChatItem;
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