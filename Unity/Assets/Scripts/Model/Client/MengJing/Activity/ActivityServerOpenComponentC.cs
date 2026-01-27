using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ActivityServerOpenComponentC : Entity, IAwake, IDestroy
    {
        public List<int> ReceivedServerOpenRewardIds { get; set; } = new();
    }
}