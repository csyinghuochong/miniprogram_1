using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(UnitMoveComponent))]
    [FriendOf(typeof(UnitMoveComponent))]
    public static partial class UnitMoveComponentSystem
    {
        [Invoke(TimerInvokeType.UnitMoveTimer)]
        public class UnitMoveTimer : ATimer<UnitMoveComponent>
        {
            protected override void Run(UnitMoveComponent self)
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
        private static void Awake(this UnitMoveComponent self)
        {
            self.Unit = self.GetParent<Unit>();
        }

        [EntitySystem]
        private static void Destroy(this UnitMoveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Update(this UnitMoveComponent self)
        {
            if (self.Targets.Count == 0)
            {
                self.EndTimer();
                return;
            }

            float3 target = self.Targets[0];
            float3 direction = target - self.Unit.Position;
            float distanceToTarget = math.length(direction);

            // TODO 优化 太小了会来回的在目标点晃动
            if (distanceToTarget <= 0.5f)
            {
                self.Unit.Position = target;
                self.Targets.RemoveAt(0);
            }
            
            if (self.Targets.Count == 0)
            {
                return;
            }

            self.Unit.GetComponent<ColliderComponent>()?.SetVelocityInDirection(new Vector2(direction.x, direction.y), self.Speed);
        }

        public static bool IsArrived(this UnitMoveComponent self)
        {
            return self.Targets.Count == 0;
        }

        private static void StartTimer(this UnitMoveComponent self)
        {
            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.UnitMoveTimer, self);
            }
        }

        private static void EndTimer(this UnitMoveComponent self)
        {
            self.Unit.GetComponent<ColliderComponent>()?.SetVelocityInDirection(Vector2.Zero, self.Speed);

            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            
            self.Unit.SendStop(0);
        }

        public static void MoveTo(this UnitMoveComponent self, float3 target, float speed)
        {
            self.Targets.Clear();

            self.Targets.Add(target);

            self.Speed = speed;

            self.StartTimer();
        }

        public static void MoveTo(this UnitMoveComponent self, List<float3> target, float speed)
        {
            self.Targets.Clear();

            foreach (float3 point in target)
            {
                self.Targets.Add(point);
            }

            self.Speed = speed;

            self.StartTimer();
        }

        public static void ChangeSpeed(this UnitMoveComponent self, float speed)
        {
            self.Speed = speed;
        }

        public static void Stop(this UnitMoveComponent self)
        {
            self.Targets.Clear();

            self.EndTimer();
        }
    }
}