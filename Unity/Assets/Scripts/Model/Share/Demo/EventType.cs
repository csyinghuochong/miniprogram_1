using System.Collections.Generic;

namespace ET
{
    public struct UpdateTimeScale
    {
        public float TimeScale;
    }

    public struct SceneChangeStart
    {
        public Scene RootScene;
        public MapType LastMapType;
        public int LastChapterId;
        public MapType MapType;
        public int ChapterId;
    }

    public struct SceneChangeFinish
    {
        public MapType MapType;
    }

    public struct AfterCreateClientScene
    {
    }

    public struct AfterCreateCurrentScene
    {
    }

    public struct AppStartInitFinish
    {
    }

    public struct EnterMapFinish
    {
    }

    public struct AfterUnitCreate
    {
        public Unit Unit;
    }

    public struct UnitRemove
    {
        public List<long> RemoveIds;
    }

    public struct UnitDead
    {
        public bool Wait;
        public Unit Unit;
    }

    public struct UnitRevive
    {
        public Unit Unit;
    }

    public struct LoginFinish
    {
    }

    public struct SessionDispose
    {
    }
}