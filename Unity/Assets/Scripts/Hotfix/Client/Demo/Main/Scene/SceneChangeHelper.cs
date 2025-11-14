namespace ET.Client
{
    public static partial class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene root, long sceneInstanceId, MapType mapType, int sceneId)
        {
            //root.RemoveComponent<AIComponent>();

            CurrentScenesComponent currentScenesComponent = root.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的
            Scene currentScene = CurrentSceneFactory.Create(sceneInstanceId, sceneId.ToString(), currentScenesComponent);
            UnitComponent unitComponent = currentScene.AddComponent<UnitComponent>();

            MapComponent mapComponent = root.GetComponent<MapComponent>();
            MapType lastMapType = mapComponent.MapType;
            int lastChapterid = mapComponent.SceneId;

            mapComponent.SetMapInfo(mapType, sceneId);

            // 可以订阅这个事件中创建Loading界面
            EventSystem.Instance.Publish(root, new SceneChangeStart()
            {
                RootScene = root,
                LastMapType = lastMapType,
                LastChapterId = lastChapterid,
                MapType = mapType,
                ChapterId = sceneId,
            });

            // 等待CreateMyUnit的消息
            Wait_CreateMyUnit waitCreateMyUnit = await root.GetComponent<ObjectWait>().Wait<Wait_CreateMyUnit>();
            M2C_CreateMyUnit m2CCreateMyUnit = waitCreateMyUnit.Message;
            Unit unit = UnitFactory.CreateUnit(currentScene, m2CCreateMyUnit.Unit, true);
            unitComponent.Add(unit);

            EventSystem.Instance.Publish(root, new SceneChangeFinish() { MapType = mapType });

            // 通知等待场景切换的协程
            root.GetComponent<ObjectWait>().Notify(new Wait_SceneChangeFinish());
        }
    }
}