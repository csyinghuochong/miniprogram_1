using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 充值积分奖励活动
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class ActivityRechargePointComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public int RechargePoint;
        public List<int> ReceivedRechargePointRewardIds = new();
    }
}