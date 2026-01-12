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

        public static void UpdateInfo(this UIAchievementItem self, Achievement achievement)
        {
            self.Achievement = achievement;

            AchievementConfig achievementConfig = AchievementConfigCategory.Instance.Get(achievement.ConfigId);
            self.Text_Name.SetText(achievementConfig.Name);
            self.Text_Description.SetText(achievementConfig.Description);
            self.Text_RewardPoints.SetTextFormat("成就点数：{0}", achievementConfig.RewardPoints);
            self.Text_Progress.SetTextFormat("进度：{0}/{1}", achievement.Progress, achievementConfig.TargetValue);

            self.GameObject_Completed.SetActive(achievement.IsCompleted == 1);
            self.Text_Progress.gameObject.SetActive(achievement.IsCompleted == 0);
        }
    }
}