using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ActivityMonthSignInComponentC : Entity, IAwake, IDestroy
    {
        public long LastSignInTime { get; set; }
        public int TotalSignInDay { get; set; }
        public List<int> ReceivedMonthSignInIds { get; set; } = new();
    }
}