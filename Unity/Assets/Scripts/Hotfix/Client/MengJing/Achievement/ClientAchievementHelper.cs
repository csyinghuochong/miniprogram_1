namespace ET.Client
{
    public static class ClientAchievementHelper
    {
        public static async ETTask<int> GetAllAchievement(Scene root)
        {
            C2M_GetAllAchievement request = C2M_GetAllAchievement.Create();

            M2C_GetAllAchievement response = (M2C_GetAllAchievement)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                AchievementComponentC achievementComponent = root.GetComponent<AchievementComponentC>();
                achievementComponent.Clear();
                achievementComponent.ReceivedAchievementRewardIds.AddRange(response.ReceivedAchievementRewardIds);
                foreach (AchievementInfo achievementInfo in response.AchievementInfoList)
                {
                    achievementComponent.AddOrUpdateAchievement(achievementInfo);
                }
            }

            return response.Error;
        }

        public static async ETTask<int> ReceivedAchievementReward(Scene root, int rewardId)
        {
            C2M_ReceivedAchievementReward request = C2M_ReceivedAchievementReward.Create();
            request.RewardId = rewardId;

            M2C_ReceivedAchievementReward response = (M2C_ReceivedAchievementReward)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                AchievementComponentC achievementComponent = root.GetComponent<AchievementComponentC>();
                achievementComponent.ReceivedAchievementRewardIds.Add(rewardId);
            }

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response.Error;
        }
    }
}