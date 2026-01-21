namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_HeroRecycleHandler : MessageLocationHandler<Unit, C2M_HeroRecycle, M2C_HeroRecycle>
    {
        protected override async ETTask Run(Unit unit, C2M_HeroRecycle request, M2C_HeroRecycle response)
        {
            await ETTask.CompletedTask;
        }
    }
}