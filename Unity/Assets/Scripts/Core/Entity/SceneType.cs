using System;

namespace ET
{
	[Flags]
	public enum SceneType: long
	{
		None = 0,
		Main = 1, // 主纤程,一个进程一个, 初始化从这里开始
		NetInner = 1 << 2, // 负责进程间消息通信
		Realm = 1 << 3,
		Gate = 1 << 4,
		Http = 1 << 5,
		Location = 1 << 6,
		Map = 1 << 7,
		Router = 1 << 8,
		RouterManager = 1 << 9,
		Robot = 1 << 10,
		BenchmarkClient = 1 << 11,
		BenchmarkServer = 1 << 12,
		Match = 1 << 13,
		Room = 1 << 14,
		LockStepClient = 1 << 15,
		LockStepServer = 1 << 16,
		RoomRoot = 1 << 17,
		Watcher = 1 << 18,
        DBCache = 1 << 20,
        Queue = 1L << 24,
        LoginCenter = 1L << 26,
        RobotManager = 1L << 28,

        // 客户端
        Demo = 1L << 50,
		Current = 1L << 51,
		LockStep = 1L << 52,
		LockStepView = 1L << 53,
		DemoView = 1L << 54,
		NetClient = 1L << 55,

        All = long.MaxValue,
	}

	public static class SceneTypeHelper
	{
		public static bool HasSameFlag(this SceneType a, SceneType b)
		{
			if (((ulong) a & (ulong) b) == 0)
			{
				return false;
			}
			return true;
		}
	}
}