namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_AchievementUpdateHandler : MessageHandler<Scene, M2C_AchievementUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_AchievementUpdate message)
        {
            AchievementComponentC achievementComponent = root.GetComponent<AchievementComponentC>();
            foreach (AchievementInfo achievementInfo in message.AchievementInfoList)
            {
                achievementComponent?.AddOrUpdateAchievement(achievementInfo);
            }

            await ETTask.CompletedTask;
        }
    }
}