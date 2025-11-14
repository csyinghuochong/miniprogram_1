namespace ET
{
    public enum MapType
    {
        Init = 0,
        Login = 1, //登录scene
        CreateRole = 2, //创角
        MainCity = 3, //主城
        LocalLevel = 4, //闯关
    }

    [ComponentOf(typeof(Scene))]
    public class MapComponent : Entity, IAwake
    {
        public int SceneId { set; get; }

        public MapType MapType { set; get; }

        public long LastQuitTime { set; get; }

        public int SonSceneId { set; get; }

        public int NavMeshId { set; get; }

        public string ParamInfo { set; get; }
    }
}