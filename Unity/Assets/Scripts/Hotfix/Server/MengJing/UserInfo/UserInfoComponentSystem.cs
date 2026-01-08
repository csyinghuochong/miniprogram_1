namespace ET.Server
{
    [EntitySystemOf(typeof(UserInfoComponent))]
    [FriendOf(typeof(UserInfoComponent))]
    public static partial class UserInfoComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UserInfoComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this UserInfoComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this UserInfoComponent self)
        {
        }

        public static string GetPlayerName(this UserInfoComponent self)
        {
            return self.PlayerName;
        }

        public static long GetGold(this UserInfoComponent self)
        {
            return self.Gold;
        }

        public static long GetDiamond(this UserInfoComponent self)
        {
            return self.Diamond;
        }

        public static int GetLv(this UserInfoComponent self)
        {
            return self.Lv;
        }

        // 直接设置
        public static void UpdateRoleData(this UserInfoComponent self, UserDataType type, string value, bool notice = true)
        {
            switch (type)
            {
                case UserDataType.PlayerName:
                    self.PlayerName = value;
                    break;
                default:
                    return;
            }

            if (notice)
            {
                M2C_RoleDataUpdate m2C_RoleDataUpdate = M2C_RoleDataUpdate.Create();
                m2C_RoleDataUpdate.UpdateType = (int)type;
                m2C_RoleDataUpdate.UpdateTypeValue = value;
                MapMessageHelper.SendToClient(self.GetParent<Unit>(), m2C_RoleDataUpdate);
            }
        }

        // 加上
        public static void ChangeRoleData(this UserInfoComponent self, UserDataType type, long value, bool notice = true)
        {
            long newValue = 0;
            switch (type)
            {
                case UserDataType.Gold:
                    self.Gold += value;
                    newValue = self.Gold;
                    break;
                case UserDataType.Diamond:
                    self.Diamond += value;
                    newValue = self.Diamond;
                    break;
                case UserDataType.Exp:
                    self.AddExp((int)value);
                    newValue = self.Exp;
                    break;
                case UserDataType.Lv:
                    self.Lv += (int)value;
                    newValue = self.Lv;
                    EventSystem.Instance.Publish(self.Scene(), new TriggerTask()
                    {
                        Unit = self.GetParent<Unit>(), TargetType = TaskTargetType.PlayerLv, TargetId = 0, TargetValue = self.Lv
                    });
                    break;
                default:
                    return;
            }

            if (notice)
            {
                M2C_RoleDataUpdate m2C_RoleDataUpdate = M2C_RoleDataUpdate.Create();
                m2C_RoleDataUpdate.UpdateType = (int)type;
                m2C_RoleDataUpdate.UpdateValueLong = newValue;
                MapMessageHelper.SendToClient(self.GetParent<Unit>(), m2C_RoleDataUpdate);
            }
        }

        private static void AddExp(this UserInfoComponent self, int value)
        {
            self.Exp += value;

            for (int i = 0; i < 99999; i++)
            {
                ExpConfig expConfig = ExpConfigCategory.Instance.Get(self.Lv);

                if (self.Exp < expConfig.PlayerUpExp)
                {
                    break;
                }

                int nextLv = self.Lv + 1;
                if (!ExpConfigCategory.Instance.DataMap.ContainsKey(nextLv) || ExpConfigCategory.Instance.Get(nextLv).PlayerUpExp == 0)
                {
                    self.Exp = expConfig.PlayerUpExp;
                    break;
                }

                self.Exp -= expConfig.PlayerUpExp;
                self.ChangeRoleData(UserDataType.Lv, 1);
            }
        }
    }
}