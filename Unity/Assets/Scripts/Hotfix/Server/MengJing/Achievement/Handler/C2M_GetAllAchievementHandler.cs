namespace ET.Server
{
    [FriendOf(typeof(AchievementComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GetAllAchievementHandler : MessageLocationHandler<Unit, C2M_GetAllAchievement, M2C_GetAllAchievement>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllAchievement request, M2C_GetAllAchievement response)
        {
            AchievementComponent achievementComponent = unit.GetComponent<AchievementComponent>();

            response.ReceivedAchievementRewardIds.AddRange(achievementComponent.ReceivedAchievementRewardIds);
            foreach (Achievement achievement in achievementComponent.AchievementList)
            {
                response.AchievementInfoList.Add(achievement.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}