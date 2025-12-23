using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFriendComponent))]
    [FriendOf(typeof(UIFriendComponent))]
    public static partial class UIFriendComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIFriendComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Scroll_FriendItem = rc.Get<GameObject>("Scroll_FriendItem");
            self.Content_UIFriendItem = rc.Get<GameObject>("Content_UIFriendItem").transform;
            self.UIFriendItem = rc.Get<GameObject>("UIFriendItem");
            self.Scroll_FriendRequestItem = rc.Get<GameObject>("Scroll_FriendRequestItem");
            self.Content_UIFriendRequestItem = rc.Get<GameObject>("Content_UIFriendItem").transform;
            self.UIFriendRequestItem = rc.Get<GameObject>("UIFriendRequestItem");
            self.Button_Type_GameFriend = rc.Get<GameObject>("Button_Type_GameFriend").GetComponent<Button>();
            self.Button_Type_FriendRequest = rc.Get<GameObject>("Button_Type_FriendRequest").GetComponent<Button>();
            self.Button_Type_Black = rc.Get<GameObject>("Button_Type_Black").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIFriend); });
            self.Button_Type_GameFriend.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_FriendRequest.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_Black.AddListener(() => { self.SetShowType(2); });

        }

        private static void SetShowType(this UIFriendComponent self, int page)
        {
            self.CurrentPage = page;
            self.Button_Type_GameFriend.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_GameFriend.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_FriendRequest.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_FriendRequest.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_Black.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_Black.transform.Find("Image_Off").gameObject.SetActive(page != 2);

            self.UpdateItemList(page);
        }
        
        public static void UpdateItemList(this UIFriendComponent self, int page)
        {
            
        }
    }
}