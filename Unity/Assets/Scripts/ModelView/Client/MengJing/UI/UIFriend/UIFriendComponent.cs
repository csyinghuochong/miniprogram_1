using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIFriendComponent : Entity, IAwake
    {
        public int CurrentPage { get; set; } = 0;

        public List<UIFriendItem> UIFriendItemList { get; set; } = new();
        public List<UIFriendRequestItem> UIFriendRequestItemList { get; set; } = new();

        public Button Button_Close;
        public GameObject Scroll_FriendItem;
        public Transform Content_UIFriendItem;
        public GameObject UIFriendItem;
        public GameObject Scroll_FriendRequestItem;
        public Transform Content_UIFriendRequestItem;
        public GameObject UIFriendRequestItem;
        public Button Button_Type_GameFriend;
        public Button Button_Type_FriendRequest;
        public Button Button_Type_Black;
    }
}