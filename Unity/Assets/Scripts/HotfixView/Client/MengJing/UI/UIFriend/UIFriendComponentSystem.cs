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
            self.UIFriendItem.SetActive(false);
            self.Scroll_FriendRequestItem = rc.Get<GameObject>("Scroll_FriendRequestItem");
            self.Content_UIFriendRequestItem = rc.Get<GameObject>("Content_UIFriendRequestItem").transform;
            self.UIFriendRequestItem = rc.Get<GameObject>("UIFriendRequestItem");
            self.UIFriendRequestItem.SetActive(false);
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
            self.Scroll_FriendItem.gameObject.SetActive(page == 0);
            self.Button_Type_FriendRequest.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_FriendRequest.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Scroll_FriendRequestItem.gameObject.SetActive(page == 1);
            self.Button_Type_Black.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_Black.transform.Find("Image_Off").gameObject.SetActive(page != 2);

            if (page == 0)
            {
                self.UpdateFriendItemList();
            }
            else if (page == 1)
            {
                self.UpdateFriendRequestItemList();
            }
            else
            {
                self.UpdateBlackItemList();
            }
        }

        public static void UpdateFriendItemList(this UIFriendComponent self)
        {
            FriendComponentC friendComponent = self.Root().GetComponent<FriendComponentC>();
            List<EntityRef<FriendDate>> friendDataList = friendComponent.FriendList;

            while (self.UIFriendItemList.Count < friendDataList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIFriendItem, self.Content_UIFriendItem);
                UIFriendItem newItem = self.AddChild<UIFriendItem, GameObject>(go);
                self.UIFriendItemList.Add(newItem);
            }

            for (int i = 0; i < friendDataList.Count; i++)
            {
                self.UIFriendItemList[i].UpdateInfo(friendDataList[i]);
                self.UIFriendItemList[i].GameObject.SetActive(true);
            }

            for (int i = friendDataList.Count; i < self.UIFriendItemList.Count; i++)
            {
                self.UIFriendItemList[i].GameObject.SetActive(false);
            }
        }

        public static void UpdateFriendRequestItemList(this UIFriendComponent self)
        {
            FriendComponentC friendComponent = self.Root().GetComponent<FriendComponentC>();
            List<EntityRef<FriendDate>> friendDataList = friendComponent.RequestList;

            while (self.UIFriendRequestItemList.Count < friendDataList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIFriendRequestItem, self.Content_UIFriendRequestItem);
                UIFriendRequestItem newItem = self.AddChild<UIFriendRequestItem, GameObject>(go);
                self.UIFriendRequestItemList.Add(newItem);
            }

            for (int i = 0; i < friendDataList.Count; i++)
            {
                self.UIFriendRequestItemList[i].UpdateInfo(friendDataList[i]);
                self.UIFriendRequestItemList[i].GameObject.SetActive(true);
            }

            for (int i = friendDataList.Count; i < self.UIFriendRequestItemList.Count; i++)
            {
                self.UIFriendRequestItemList[i].GameObject.SetActive(false);
            }
        }

        public static void UpdateBlackItemList(this UIFriendComponent self)
        {
        }
    }
}