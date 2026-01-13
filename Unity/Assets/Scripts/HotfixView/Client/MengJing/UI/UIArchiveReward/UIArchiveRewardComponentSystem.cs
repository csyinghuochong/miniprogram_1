using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIArchiveRewardComponent))]
    [FriendOf(typeof(UIArchiveRewardComponent))]
    public static partial class UIArchiveRewardComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIArchiveRewardComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIArchiveRewardItem = rc.Get<GameObject>("Content_UIArchiveRewardItem").transform;
            self.UIArchiveRewardItem = rc.Get<GameObject>("UIArchiveRewardItem");
            self.Text_CurrentPoints = rc.Get<GameObject>("Text_CurrentPoints").GetComponent<TMP_Text>();

            self.UIArchiveRewardItem.gameObject.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIArchiveReward); });

            self.UpdateInfo();
        }

        [EntitySystem]
        private static void Destroy(this UIArchiveRewardComponent self)
        {
            self.UIArchiveRewardItemList.Clear();
            self.UIArchiveRewardItem = null;
        }

        public static void UpdateInfo(this UIArchiveRewardComponent self)
        {
            List<ArchiveRewardConfig> archiveRewardConfigs = ArchiveRewardConfigCategory.Instance.DataList;

            while (self.UIArchiveRewardItemList.Count < archiveRewardConfigs.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIArchiveRewardItem, self.Content_UIArchiveRewardItem);
                UIArchiveRewardItem newItem = self.AddChild<UIArchiveRewardItem, GameObject>(go);
                self.UIArchiveRewardItemList.Add(newItem);
            }

            for (int i = 0; i < archiveRewardConfigs.Count; i++)
            {
                self.UIArchiveRewardItemList[i].UpdateInfo(archiveRewardConfigs[i].Id);
                self.UIArchiveRewardItemList[i].GameObject.SetActive(true);
            }

            for (int i = archiveRewardConfigs.Count; i < self.UIArchiveRewardItemList.Count; i++)
            {
                self.UIArchiveRewardItemList[i].GameObject.SetActive(false);
            }

            ArchiveComponentC archiveComponent = self.Root().GetComponent<ArchiveComponentC>();
            int currentPoint = archiveComponent.GetCurrentPoint();
            self.Text_CurrentPoints.SetTextFormat("当前积分：{0}", currentPoint);
        }
    }
}