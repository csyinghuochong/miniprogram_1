using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(PickUpDropItemComponent))]
    [FriendOf(typeof(PickUpDropItemComponent))]
    public static partial class PickUpDropItemComponentSystem
    {
        [Invoke(TimerInvokeType.PickUpDropItemTimer)]
        public class PickUpDropItemTimer : ATimer<PickUpDropItemComponent>
        {
            protected override void Run(PickUpDropItemComponent self)
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
        private static void Awake(this PickUpDropItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this PickUpDropItemComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.DropItemList.Clear();
        }

        private static void Update(this PickUpDropItemComponent self)
        {
            if (self.DropItemList.Count == 0 && self.SendIdList.Count == 0)
            {
                self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
                return;
            }

            if (self.MainUnit == null)
            {
                return;
            }

            const float speed = 12f;

            for (int i = self.DropItemList.Count - 1; i >= 0; i--)
            {
                Unit unit = self.DropItemList[i];

                float3 direction = self.MainUnit.Position - unit.Position;
                float distanceToTarget = math.length(direction);
                float moveStep = speed * Time.deltaTime;

                if (distanceToTarget <= moveStep)
                {
                    self.DropItemList.RemoveAt(i);
                    self.SendIdList.Add(unit.Id);
                }
                else
                {
                    unit.Position += math.normalize(direction) * moveStep;

                    float rotationAngle = 0;
                    if (distanceToTarget > 0.001f)
                    {
                        float radian = math.atan2(direction.y, direction.x);
                        rotationAngle = math.degrees(radian);
                    }
                }
            }

            long now = TimeHelper.ServerNow();
            if (self.SendIdList.Count > 0 && self.LastSendTime + 200 < now)
            {
                ClientLevelHelper.PickUpDropItem(self.Root(), self.SendIdList).Coroutine();
                self.LastSendTime = now;
                self.SendIdList.Clear();
            }
        }

        public static void OnStarDrop(this PickUpDropItemComponent self)
        {
            self.DropItemList.Clear();
            self.SendIdList.Clear();

            if (self.MainUnit == null)
            {
                self.MainUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());
            }
            
            UnitComponent unitComponent = self.MainUnit.GetParent<UnitComponent>();
            foreach (Unit unit in unitComponent.GetAll())
            {
                if (unit.Type == UnitType.DropItem)
                {
                    self.DropItemList.Add(unit);
                }
            }

            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.PickUpDropItemTimer, self);
            }
        }
    }
}