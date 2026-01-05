using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRankComponent))]
    [FriendOf(typeof(UIRankComponent))]
    public static partial class UIRankComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIRankComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Type_CE = rc.Get<GameObject>("Button_Type_CE").GetComponent<Button>();
            self.Button_Type_LianMeng = rc.Get<GameObject>("Button_Type_LianMeng").GetComponent<Button>();
            self.Scroll_UIRankCEItem = rc.Get<GameObject>("Scroll_UIRankCEItem");
            self.Content_UIRankCEItem = rc.Get<GameObject>("Content_UIRankCEItem").transform;
            self.UIRankCEItem = rc.Get<GameObject>("UIRankCEItem");
            self.UIRankCEItem.SetActive(false);
            self.Scroll_UIRankLianMengItem = rc.Get<GameObject>("Scroll_UIRankLianMengItem");
            self.Content_UIRankLianMengItem = rc.Get<GameObject>("Content_UIRankLianMengItem").transform;
            self.UIRankLianMengItem = rc.Get<GameObject>("UIRankLianMengItem");
            self.UIRankLianMengItem.SetActive(false);
            self.Image_SelfHead = rc.Get<GameObject>("Image_SelfHead").GetComponent<Image>();
            self.Button_OnSelfHead = rc.Get<GameObject>("Button_OnSelfHead").GetComponent<Button>();
            self.Text_SelfName = rc.Get<GameObject>("Text_SelfName").GetComponent<TMP_Text>();
            self.Text_SelfCE = rc.Get<GameObject>("Text_SelfCE").GetComponent<TMP_Text>();
            self.Text_SelfSort = rc.Get<GameObject>("Text_SelfSort").GetComponent<TMP_Text>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIRank); });
            self.Button_Type_CE.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_LianMeng.onClick.AddListener(() => { self.SetShowType(1); });
        }

        public static void SetShowType(this UIRankComponent self, int page)
        {
            self.CurrentPage = page;
            self.Button_Type_CE.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_CE.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Scroll_UIRankCEItem.gameObject.SetActive(page == 0);
            self.Button_Type_LianMeng.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_LianMeng.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Scroll_UIRankLianMengItem.gameObject.SetActive(page == 1);

            if (page == 0)
            {
                self.UpdateRankCEItemList();
            }
            else if (page == 1)
            {
                self.UpdateRankLianMengItemList();
            }
        }

        public static void UpdateRankCEItemList(this UIRankComponent self)
        {
            RankComponent rankComponent = self.Root().GetComponent<RankComponent>();
            List<RankData> playerRankDataList = rankComponent.GetPlayerRankDataList();

            while (self.UIRankCEItemList.Count < playerRankDataList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIRankCEItem, self.Content_UIRankCEItem);
                UIRankCEItem newItem = self.AddChild<UIRankCEItem, GameObject>(go);
                self.UIRankCEItemList.Add(newItem);
            }

            for (int i = 0; i < playerRankDataList.Count; i++)
            {
                self.UIRankCEItemList[i].UpdateInfo(playerRankDataList[i]);
                self.UIRankCEItemList[i].GameObject.SetActive(true);
            }

            for (int i = playerRankDataList.Count; i < self.UIRankCEItemList.Count; i++)
            {
                self.UIRankCEItemList[i].GameObject.SetActive(false);
            }
        }

        public static void UpdateRankLianMengItemList(this UIRankComponent self)
        {
        }
    }
}