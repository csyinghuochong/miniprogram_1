namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_UnitBuffRemoveHandler : MessageHandler<Scene, M2C_UnitBuffRemove>
    {
        protected override async ETTask Run(Scene root, M2C_UnitBuffRemove message)
        {
            using var _ = message;
            
            Unit unit = root.CurrentScene()?.GetComponent<UnitComponent>().Get(message.UnitId);
            if (unit == null)
            {
                return;
            }

            //移除
            unit.GetComponent<BuffManagerComponentC>().RemoveBuff(message.BuffId);

            await ETTask.CompletedTask;
        }
    }
}