using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(PickUpDropItemComponent))]
    [FriendOf(typeof(PickUpDropItemComponent))]
    public static partial class PickUpDropItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PickUpDropItemComponent self)
        {
            self.MainUnit = self.GetParent<Unit>();
        }

        [EntitySystem]
        private static void Update(this PickUpDropItemComponent self)
        {
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
                    ClientLevelHelper.PickUpDropItem(self.Root(), unit.Id).Coroutine();
                    self.DropItemList.RemoveAt(i);
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
        }

        [EntitySystem]
        private static void Destroy(this PickUpDropItemComponent self)
        {
            self.DropItemList.Clear();
        }

        public static void OnStarDrop(this PickUpDropItemComponent self)
        {
            self.DropItemList.Clear();

            UnitComponent unitComponent = self.MainUnit.GetParent<UnitComponent>();
            foreach (Unit unit in unitComponent.GetAll())
            {
                if (unit.Type == UnitType.DropItem)
                {
                    self.DropItemList.Add(unit);
                }
            }
        }
    }
}