using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [EntitySystemOf(typeof(Move2DComponent))]
    [FriendOf(typeof(Move2DComponent))]
    public static partial class Move2DComponentSystem
    {
        [Invoke(TimerInvokeType.Move2DTimer)]
        public class Move2DTimer : ATimer<Move2DComponent>
        {
            protected override void Run(Move2DComponent self)
            {
                try
                {
                    self.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        [EntitySystem]
        private static void Awake(this Move2DComponent self)
        {
            self.Unit = self.GetParent<Unit>();

#if DOTNET
            self.TimeInterval = 100;
#else
            self.TimeInterval = 20;
#endif
        }

        [EntitySystem]
        private static void Destroy(this Move2DComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Update(this Move2DComponent self)
        {
            if (self.Targets.Count == 0)
            {
                self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
                return;
            }

            float deltaTime = self.TimeInterval / 1000f * self.Scene().TimeScale;

            float3 target = self.Targets[0];
            float3 direction = target - self.Unit.Position;
            float distanceToTarget = math.length(direction);
            float moveStep = self.Speed * deltaTime;

            if (distanceToTarget <= moveStep)
            {
                self.Unit.Position = target;
                self.Targets.RemoveAt(0);
            }
            else
            {
                self.Unit.Position += math.normalize(direction) * moveStep;
            }
        }

        public static bool IsArrived(this Move2DComponent self)
        {
            return self.Targets.Count == 0;
        }

        public static void MoveTo(this Move2DComponent self, float3 target, float speed)
        {
            self.Targets.Clear();

            self.Targets.Add(target);

            self.Speed = speed;

            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(self.TimeInterval, TimerInvokeType.Move2DTimer, self);
            }
        }

        public static void MoveTo(this Move2DComponent self, List<float3> target, float speed)
        {
            self.Targets.Clear();

            foreach (float3 point in target)
            {
                self.Targets.Add(point);
            }

            self.Speed = speed;

            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(self.TimeInterval, TimerInvokeType.Move2DTimer, self);
            }
        }

        public static void Stop(this Move2DComponent self)
        {
            self.Targets.Clear();

            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        // 距离差距过大，直接同步，否则平滑移动过去
        public static void Stop(this Move2DComponent self, float3 target)
        {
            float3 direction = target - self.Unit.Position;
            float distanceToTarget = math.length(direction);
            if (distanceToTarget > 5f)
            {
                self.Targets.Clear();
                self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);

                self.Unit.Position = target;
            }
            else
            {
                self.Targets.Clear();
                self.Targets.Add(target);
            }
        }
    }
}