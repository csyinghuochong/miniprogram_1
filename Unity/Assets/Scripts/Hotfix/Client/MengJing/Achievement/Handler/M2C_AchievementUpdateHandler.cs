namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_AchievementUpdateHandler : MessageHandler<Scene, M2C_AchievementUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_AchievementUpdate message)
        {
            await ETTask.CompletedTask;
        }
    }
}