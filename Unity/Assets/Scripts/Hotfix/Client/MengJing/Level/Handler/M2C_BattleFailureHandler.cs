namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_BattleFailureHandler : MessageHandler<Scene, M2C_BattleFailure>
    {
        protected override async ETTask Run(Scene root, M2C_BattleFailure message)
        {
            EventSystem.Instance.Publish(root, new BattleFailure());

            await ETTask.CompletedTask;
        }
    }
}