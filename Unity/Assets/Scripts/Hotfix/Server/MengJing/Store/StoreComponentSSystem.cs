using System;

namespace ET.Server
{
    [EntitySystemOf(typeof(StoreComponentS))]
    [FriendOf(typeof(StoreComponentS))]
    public static partial class StoreComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this StoreComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this StoreComponentS self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this StoreComponentS self)
        {
        }

        public static void Check(this StoreComponentS self)
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

        public static void RefreshStore(this StoreComponentS self)
        {
            self.StoreItemList.Clear();

            foreach (StoreItemConfig storeItemConfig in StoreItemConfigCategory.Instance.DataList)
            {
                self.StoreItemList.Add(storeItemConfig.Id, storeItemConfig.LimitNumber);
            }
        }
    }
}