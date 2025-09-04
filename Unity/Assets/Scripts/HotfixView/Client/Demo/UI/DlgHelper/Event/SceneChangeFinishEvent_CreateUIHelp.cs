namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinishEvent_CreateUIHelp : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
             scene.AddComponent<MJCameraComponent>();
            
             scene.AddComponent<OperaComponent>();

             // scene.Root().GetComponent<UIComponent>().GetDlgLogic<DlgMain>()?.InitMainHero(args.SceneType);
             
             await ETTask.CompletedTask;
        }
    }
}