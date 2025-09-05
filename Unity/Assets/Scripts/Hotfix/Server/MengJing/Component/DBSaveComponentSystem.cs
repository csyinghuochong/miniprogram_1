using System;

namespace ET.Server
{
    [EntitySystemOf((typeof(DBSaveComponent)))]
    [FriendOf((typeof(DBSaveComponent)))]
    public static partial class DBSaveComponentSystem
    {
        [Invoke(TimerInvokeType.DBSaveTimer)]
        public class DBSaveTimer : ATimer<DBSaveComponent>
        {
            protected override void Run(DBSaveComponent self)
            {
                try
                {
                    self.Check();
                }
                catch (Exception e)
                {
                    Log.Error($"session idle checker timer error: {self.Id}\n{e}");
                }
            }
        }

        [EntitySystem]
        private static void Awake(this DBSaveComponent self)
        {
        }

        public static void SetNoFindPath(this DBSaveComponent self)
        {
        }

        public static void OnLogin(this DBSaveComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            string offLineInfo = $"{unit.Zone()}区： " +
                    $"unit.id: {unit.GetComponent<UserInfoComponentS>().Id} : " +
                    $" {unit.GetComponent<UserInfoComponentS>().PlayerName} : " +
                    $"{TimeHelper.DateTimeNow().ToString()}   登录";
            Log.Debug(offLineInfo);

            self.PlayerState = PlayerState.Game;
            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();
            numericComponent.ApplyValue(NumericType.LastLoginTime, TimeHelper.ServerNow(), false);
        }

        public static void OnRelogin(this DBSaveComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            string offLineInfo = $"{unit.Zone()}区： " +
                    $"unit.id: {unit.Id} : " +
                    $" {unit.GetComponent<UserInfoComponentS>().PlayerName} : " +
                    $"{TimeHelper.DateTimeNow().ToString()}   二次登录";
            Log.Debug(offLineInfo);

            self.PlayerState = PlayerState.Game;
        }

        //离线
        public static void OnOffLine(this DBSaveComponent self)
        {
            Console.WriteLine($"OnOffLine: {self.Id}");

            Unit unit = self.GetParent<Unit>();
            string offLineInfo = $"{unit.Zone()}区： " +
                    $"unit.id: {unit.Id} : " +
                    $" {unit.GetComponent<UserInfoComponentS>().PlayerName} : " +
                    $"{TimeHelper.DateTimeNow().ToString()}   离线";
            Log.Debug(offLineInfo);

            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();
            numericComponent.ApplyValue(NumericType.LastLoginTime, TimeHelper.ServerNow(), false);
            self.PlayerState = PlayerState.Disconnect;

            self.UpdateCacheDB();
        }

        public static void OnDisconnect(this DBSaveComponent self)
        {
            Console.WriteLine($"OnDisconnect: {self.Id}");

            Unit unit = self.GetParent<Unit>();
            string offLineInfo = $"{unit.Zone()}区： " +
                    $"unit.id: {unit.Id} : " +
                    $" {unit.GetComponent<UserInfoComponentS>().PlayerName} : " +
                    $"{TimeHelper.DateTimeNow().ToString()}  移除";
            Log.Debug(offLineInfo);

            self.UpdateCacheDB();
            unit.GetParent<UnitComponent>().Remove(unit.Id);
            TransferHelper.OnPlayerDisconnect(unit.Scene(), unit.Id);
        }

        public static void Activeted(this DBSaveComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(TimeHelper.Second, TimerInvokeType.DBSaveTimer, self);
        }

        public static void UpdateCacheDB(this DBSaveComponent self)
        {
            UnitCacheHelper.AddOrUpdateUnitAllCache(self.GetParent<Unit>());
        }

        /// <summary>
        /// 每秒检测
        /// </summary>
        /// <param name="self"></param>
        public static void Check(this DBSaveComponent self)
        {
            Unit unit = self.GetParent<Unit>();

            self.DBInterval++;
            // 开发 60   正式 600
            if (self.DBInterval >= 60)
            {
                self.DBInterval = 0;
                self.UpdateCacheDB();
            }
        }

        [EntitySystem]
        private static void Destroy(this DBSaveComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
        }
    }
}