namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class AppStartInitFinish_CreateLoginUI : AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            MapComponent mapComponent = root.GetComponent<MapComponent>();
            mapComponent.MapType = MapType.Login;

            await root.GetComponent<UIComponent>().Create(UIType.UILogin);
            await root.GetComponent<SceneManagerComponent>().ChangeScene(MapType.Login, 0, 0);
        }
    }
}