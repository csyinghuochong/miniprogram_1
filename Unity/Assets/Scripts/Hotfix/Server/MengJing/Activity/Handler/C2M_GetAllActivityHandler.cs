namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GetAllActivityHandler : MessageLocationHandler<Unit, C2M_GetAllActivity, M2C_GetAllActivity>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllActivity request, M2C_GetAllActivity response)
        {
            await ETTask.CompletedTask;
        }
    }
}