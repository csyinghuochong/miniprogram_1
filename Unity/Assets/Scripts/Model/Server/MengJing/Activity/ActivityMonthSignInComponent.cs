using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 签到活动
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class ActivityMonthSignInComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public long LastSignInTime;
        public int TotalSignInDay;
        public List<int> ReceivedMonthSignInIds = new();
    }
}