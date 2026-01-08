using System;

namespace ET.Server
{
    [EntitySystemOf(typeof(StoreComponent))]
    [FriendOf(typeof(StoreComponent))]
    public static partial class StoreComponentSystem
    {
        [EntitySystem]
        private static void Awake(this StoreComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this StoreComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this StoreComponent self)
        {
        }

        public static void Check(this StoreComponent self)
        {
            if (self.StoreItemList.Count == 0 || TimeHelper.ServerNow() > self.RefreshTime)
            {
                self.RefreshTime = GetNextDayZeroTimestamp();
                self.RefreshNum = ConfigData.StoreRefreshNum;

                self.RefreshStore();
            }
        }

        // 次日凌晨
        private static long GetNextDayZeroTimestamp()
        {
            DateTime now = TimeInfo.Instance.ToDateTime(TimeHelper.ServerNow());
            DateTime nextDayZero = now.Date.AddDays(1);
            nextDayZero = new DateTime(nextDayZero.Year, nextDayZero.Month, nextDayZero.Day, 0, 0, 0);

            return TimeInfo.Instance.Transition(nextDayZero);
        }

        public static void RefreshStore(this StoreComponent self)
        {
            self.StoreItemList.Clear();

            foreach (StoreItemConfig storeItemConfig in StoreItemConfigCategory.Instance.DataList)
            {
                self.StoreItemList.Add(storeItemConfig.Id, storeItemConfig.LimitNumber);
            }
        }
    }
}