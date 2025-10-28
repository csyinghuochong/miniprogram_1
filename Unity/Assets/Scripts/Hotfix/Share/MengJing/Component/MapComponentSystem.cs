namespace ET
{
    [EntitySystemOf(typeof(MapComponent))]
    [FriendOf(typeof(MapComponent))]
    public static partial class MapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MapComponent self)
        {
        }

        public static void SetMapInfo(this MapComponent self, int sceneTypeEnum, int mapid)
        {
            self.MapType = sceneTypeEnum;
            self.SceneId = mapid;
        }
        
        public static void SetSubLevel(this MapComponent self, int sonMapid)
        {
            self.SonSceneId = sonMapid;
        }
    }
}