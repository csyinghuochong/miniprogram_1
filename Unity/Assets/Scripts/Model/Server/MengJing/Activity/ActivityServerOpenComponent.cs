using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 开区活动
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class ActivityServerOpenComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public List<int> ReceivedServerOpenRewardIds = new();
    }
}