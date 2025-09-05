namespace ET.Client
{
    [Event(SceneType.Main)]
    public class EntryEvent3_InitClient: AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            GlobalComponent globalComponent = root.AddComponent<GlobalComponent>();
            root.AddComponent<ResourcesLoaderComponent>();
            root.AddComponent<MaskWordHelper>();
            root.AddComponent<LanguageComponent>();
            root.AddComponent<UIEventComponent>();
            root.AddComponent<UIComponent>();
            root.AddComponent<PlayerInfoComponent>();
            root.AddComponent<CurrentScenesComponent>();
            root.AddComponent<SceneManagerComponent>();
            root.AddComponent<MapComponent>();
            root.AddComponent<GameObjectLoadComponent>();
            root.AddComponent<SoundComponent>();
            root.AddComponent<ReddotComponentC>();
            root.AddComponent<ReddotViewComponent>();
            
            // 根据配置修改掉Main Fiber的SceneType
            SceneType sceneType = EnumHelper.FromString<SceneType>(globalComponent.GlobalConfig.AppType.ToString());
            root.SceneType = sceneType;

            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
        }
    }
}