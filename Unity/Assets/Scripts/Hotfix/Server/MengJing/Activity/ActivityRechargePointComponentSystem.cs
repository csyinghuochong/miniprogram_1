namespace ET.Server
{
    [FriendOf(typeof(ActivityRechargePointComponent))]
    [EntitySystemOf(typeof(ActivityRechargePointComponent))]
    public static partial class ActivityRechargePointComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ActivityRechargePointComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ActivityRechargePointComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ActivityRechargePointComponent self)
        {
        }

        public static void Recharge(this ActivityRechargePointComponent self, int recharge)
        {
            self.RechargePoint += recharge * 10;

            UserInfoComponent userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponent>();
            int oldVip = userInfoComponent.GetVipLv();
            int newVip = 1;
            for (int i = 0; i < RechargePointsRewardConfigCategory.Instance.DataList.Count; i++)
            {
                RechargePointsRewardConfig config = RechargePointsRewardConfigCategory.Instance.DataList[i];

                newVip = i + 1;
                if (self.RechargePoint < config.RequiredPoints)
                {
                    break;
                }
            }

            if (newVip > oldVip)
            {
                userInfoComponent.SetVipLv(newVip);
            }

            M2C_ActivityRechargePointUpdate message = M2C_ActivityRechargePointUpdate.Create();
            message.RechargePoint = self.RechargePoint;

            MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
        }
    }
}