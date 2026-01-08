using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRankLianMengItem))]
    [FriendOf(typeof(UIRankLianMengItem))]
    public static partial class UIRankLianMengItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIRankLianMengItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_LianMengHead = rc.Get<GameObject>("Image_LianMengHead").GetComponent<Image>();
            self.Button_OnLianMengHead = rc.Get<GameObject>("Button_OnLianMengHead").GetComponent<Button>();
            self.Text_LianMengName = rc.Get<GameObject>("Text_LianMengName").GetComponent<TMP_Text>();
            self.Text_LianMengAcive = rc.Get<GameObject>("Text_LianMengActive").GetComponent<TMP_Text>();
            self.Text_Sort = rc.Get<GameObject>("Text_Sort").GetComponent<TMP_Text>();
        }

        public static void UpdateInfo(this UIRankLianMengItem self, AllianceRank allianceRank)
        {
            self.AllianceRank = allianceRank;

            self.Text_Sort.SetText(self.AllianceRank.Sort);
            self.Text_LianMengName.SetText(self.AllianceRank.AllianceName);
            self.Text_LianMengAcive.SetTextFormat("活跃度:{0}", self.AllianceRank.Active);
        }
    }
}