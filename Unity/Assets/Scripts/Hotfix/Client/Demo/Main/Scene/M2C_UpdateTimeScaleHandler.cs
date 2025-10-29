namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    [FriendOf(typeof(Scene))]
    public class M2C_UpdateTimeScaleHandler : MessageHandler<Scene, M2C_UpdateTimeScale>
    {
        protected override async ETTask Run(Scene root, M2C_UpdateTimeScale message)
        {
            root.GetComponent<MapComponent>().TimeScale = message.TimeScale;

            await ETTask.CompletedTask;
        }
    }
}