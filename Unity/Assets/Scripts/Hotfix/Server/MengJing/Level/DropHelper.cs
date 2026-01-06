using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    public static class DropHelper
    {
        public static void MonsterDropItem(Unit unit)
        {
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);

            foreach (int dropId in monsterConfig.DropId)
            {
                RewardItem rewardItem = DropHelper.DropItem(dropId);
                float3 position = new float3(unit.Position.x + RandomHelper.RandomNumberFloat(-2, 2),
                    unit.Position.y + RandomHelper.RandomNumberFloat(-2, 2), unit.Position.z);
                UnitFactory.CreateDropItem(unit.Scene(), rewardItem.ItemId, rewardItem.ItemNum, position);
            }
        }

        public static RewardItem DropItem(int dropId)
        {
            using ListComponent<DropItemInfo> allDrop = ListComponent<DropItemInfo>.Create();
            int totalWeight = 0;
            DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);
            foreach (DropItemInfo dropItemInfo in dropConfig.DropItemInfos)
            {
                totalWeight += dropItemInfo.Weight;
                allDrop.Add(dropItemInfo);
            }

            if (allDrop.Count == 0)
            {
                return new RewardItem() { ItemId = 1, ItemNum = 1 };
            }

            int random = RandomHelper.RandomNumber(1, totalWeight + 1);
            int index = 0;
            DropItemInfo current = allDrop[0];
            foreach (DropItemInfo dropItemInfo in allDrop)
            {
                index += dropItemInfo.Weight;
                if (random <= index)
                {
                    current = dropItemInfo;
                    break;
                }
            }

            return new RewardItem() { ItemId = current.ItemId, ItemNum = RandomHelper.RandomNumber(current.MinNum, current.MaxNum + 1) };
        }

        public static RewardItem DropItem_2(int dropId, int itemId, int addWeight)
        {
            using ListComponent<DropItemInfo> allDrop = ListComponent<DropItemInfo>.Create();
            int totalWeight = 0;
            DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);
            foreach (DropItemInfo dropItemInfo in dropConfig.DropItemInfos)
            {
                totalWeight += dropItemInfo.Weight;
                if (dropItemInfo.ItemId == itemId)
                {
                    totalWeight += addWeight;
                }

                allDrop.Add(dropItemInfo);
            }

            if (allDrop.Count == 0)
            {
                return new RewardItem() { ItemId = 1, ItemNum = 1 };
            }

            int random = RandomHelper.RandomNumber(1, totalWeight + 1);
            int index = 0;
            DropItemInfo current = allDrop[0];
            foreach (DropItemInfo dropItemInfo in allDrop)
            {
                index += dropItemInfo.Weight;
                if (dropItemInfo.ItemId == itemId)
                {
                    index += addWeight;
                }

                if (random <= index)
                {
                    current = dropItemInfo;
                    break;
                }
            }

            return new RewardItem() { ItemId = current.ItemId, ItemNum = RandomHelper.RandomNumber(current.MinNum, current.MaxNum + 1) };
        }
    }
}