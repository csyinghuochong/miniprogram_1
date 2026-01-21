namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ItemRecycleHandler: MessageLocationHandler<Unit, C2M_ItemRecycle, M2C_ItemRecycle>
    {
        protected override async ETTask Run(Unit unit, C2M_ItemRecycle request, M2C_ItemRecycle response)
        {
            await ETTask.CompletedTask;
        }
    }
}