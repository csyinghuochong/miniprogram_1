namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    [FriendOf(typeof(Scene))]
    public class M2C_UpdateTimeScaleHandler : MessageHandler<Scene, M2C_UpdateTimeScale>
    {
        protected override async ETTask Run(Scene root, M2C_UpdateTimeScale message)
        {
            root.CurrentScene().TimeScale = message.TimeScale;

            EventSystem.Instance.Publish(root, new UpdateTimeScale() { TimeScale = message.TimeScale });

            await ETTask.CompletedTask;
        }
    }
}