namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeFinish_RequestInfo : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            InventoryHelper.GetAllItem(scene).Coroutine();
            HeroHelper.GetAllHero(scene).Coroutine();
            
            await ETTask.CompletedTask;
        }
    }
}