using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIAchievementItem))]
    [FriendOf(typeof(UIAchievementItem))]
    public static partial class UIAchievementItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIAchievementItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_Icon = rc.Get<GameObject>("Image_Icon").GetComponent<Image>();
            self.Text_Name = rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>();
            self.Text_Description = rc.Get<GameObject>("Text_Description").GetComponent<TMP_Text>();
            self.Text_RewardPoints = rc.Get<GameObject>("Text_RewardPoints").GetComponent<TMP_Text>();
            self.Text_Progress = rc.Get<GameObject>("Text_Progress").GetComponent<TMP_Text>();
            self.GameObject_Completed = rc.Get<GameObject>("GameObject_Completed");
        }

        public static async ETTask UpdateInfo(this UIAchievementItem self, int achieveConfigId)
        {
            self.AchieveConfigId = achieveConfigId;
            
            self.GameObject_Completed.SetActive(false);
            
        }
    }
}