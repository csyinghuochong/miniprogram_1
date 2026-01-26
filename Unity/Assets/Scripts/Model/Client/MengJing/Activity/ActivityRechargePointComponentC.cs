using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ActivityRechargePointComponentC : Entity, IAwake, IDestroy
    {
        public int RechargePoint { get; set; }
        public List<int> ReceivedRechargePointRewardIds { get; set; } = new();
    }
}