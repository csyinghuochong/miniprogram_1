using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIServerOpenRewardComponent))]
    [FriendOf(typeof(UIServerOpenRewardComponent))]
    public static partial class UIServerOpenRewardComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIServerOpenRewardComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Type_Lv = rc.Get<GameObject>("Button_Type_Lv").GetComponent<Button>();
            self.Button_Type_CE = rc.Get<GameObject>("Button_Type_CE").GetComponent<Button>();
            self.Content_UIRewardItem = rc.Get<GameObject>("Content_UIRewardItem").transform;
            self.UIRewardItem = rc.Get<GameObject>("UIRewardItem");
            self.UIRewardItem.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIServerOpenReward); });
            self.Button_Type_Lv.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_CE.AddListener(() => { self.SetShowType(2); });

            self.SetShowType(1);
        }

        [EntitySystem]
        private static void Destroy(this UIServerOpenRewardComponent self)
        {
            self.UIRewardItemList.Clear();
            self.UIRewardItem = null;
        }

        private static void SetShowType(this UIServerOpenRewardComponent self, int page)
        {
            self.CurrentPage = page;
            self.Button_Type_Lv.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_Lv.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_CE.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_CE.transform.Find("Image_Off").gameObject.SetActive(page != 2);

            self.UpdateList(page);
        }

        public static void UpdateList(this UIServerOpenRewardComponent self, int page)
        {
            List<ServerOpenRewardConfig> serverOpenRewardConfigs = ServerOpenRewardConfigCategory.Instance.DataList;
            List<ServerOpenRewardConfig> currentRewardConfigs = new List<ServerOpenRewardConfig>();

            for (int i = 0; i < serverOpenRewardConfigs.Count; i++)
            {
                if (serverOpenRewardConfigs[i].RequiredType == page)
                {
                    currentRewardConfigs.Add(serverOpenRewardConfigs[i]);
                }
            }

            while (self.UIRewardItemList.Count < currentRewardConfigs.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIRewardItem, self.Content_UIRewardItem);
                UIRewardItem newItem = self.AddChild<UIRewardItem, GameObject>(go);
                self.UIRewardItemList.Add(newItem);
            }

            for (int i = 0; i < currentRewardConfigs.Count; i++)
            {
                self.UIRewardItemList[i].UpdateInfo(currentRewardConfigs[i].Id);
                self.UIRewardItemList[i].GameObject.SetActive(true);
            }

            for (int i = currentRewardConfigs.Count; i < self.UIRewardItemList.Count; i++)
            {
                self.UIRewardItemList[i].GameObject.SetActive(false);
            }
        }
    }
}