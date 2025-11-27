namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeFinish_AddComponent : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene root, SceneChangeFinish args)
        {
            
            await ETTask.CompletedTask;
        }
    }
}