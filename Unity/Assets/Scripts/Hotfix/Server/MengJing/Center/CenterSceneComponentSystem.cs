using System;

namespace ET.Server
{
    [EntitySystemOf(typeof(CenterSceneComponent))]
    [FriendOf(typeof(CenterSceneComponent))]
    public static partial class CenterSceneComponentSystem
    {
        [Invoke(TimerInvokeType.CenterSceneTimer)]
        public class CenterSceneTimer : ATimer<CenterSceneComponent>
        {
            protected override void Run(CenterSceneComponent self)
            {
                try
                {
                    BroadCastHelper.SaveData(self.Root()).Coroutine();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        [EntitySystem]
        private static void Awake(this CenterSceneComponent self)
        {
            // 开发 60   正式 600 或者凌晨刷新
            long time = 60 * 1000;
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(time, TimerInvokeType.CenterSceneTimer, self);
        }

        [EntitySystem]
        private static void Destroy(this CenterSceneComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }
    }
}