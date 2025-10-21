namespace ET.Server
{
    [EntitySystemOf(typeof(UserInfoComponentS))]
    [FriendOf(typeof(UserInfoComponentS))]
    public static partial class UserInfoComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this UserInfoComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this UserInfoComponentS self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this UserInfoComponentS self)
        {
        }

        // 直接设置
        public static void UpdateRoleData(this UserInfoComponentS self, UserDataType type, string value, bool notice = true)
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

        // 直接设置
        public static void UpdateRoleData(this UserInfoComponentS self, UserDataType type, long value, bool notice = true)
        {
            switch (type)
            {
                case UserDataType.Gold:
                    self.Gold = value;
                    break;
                case UserDataType.Diamond:
                    self.Diamond = value;
                    break;
                case UserDataType.Exp:
                    self.Exp = value;
                    break;
                case UserDataType.Lv:
                    self.Lv = (int)value;
                    break;
                default:
                    return;
            }

            if (notice)
            {
                M2C_RoleDataUpdate m2C_RoleDataUpdate = M2C_RoleDataUpdate.Create();
                m2C_RoleDataUpdate.UpdateType = (int)type;
                m2C_RoleDataUpdate.UpdateValueLong = value;
                MapMessageHelper.SendToClient(self.GetParent<Unit>(), m2C_RoleDataUpdate);
            }
        }

        // 加上
        public static void ChangeRoleData(this UserInfoComponentS self, UserDataType type, long value, bool notice = true)
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

        private static void AddExp(this UserInfoComponentS self, int value)
        {
            self.Exp += value;

            while (true)
            {
                ExpConfig currentExpConfig = ExpConfigCategory.Instance.Get(self.Lv);

                if (self.Exp < currentExpConfig.UpExp)
                {
                    break;
                }

                int nextLv = self.Lv + 1;
                if (!ExpConfigCategory.Instance.DataMap.ContainsKey(nextLv))
                {
                    self.Exp = currentExpConfig.UpExp;
                    break;
                }

                self.Exp -= currentExpConfig.UpExp;
                self.ChangeRoleData(UserDataType.Lv, 1);
            }
        }
    }
}