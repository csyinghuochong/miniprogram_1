using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(FriendUnit))]
    public class FriendComponent : Entity, IAwake, IDestroy, IDeserialize
    {
        /// <summary>
        /// 好友列表
        /// </summary>
        public List<long> FriendList { get; set; } = new();

        /// <summary>
        /// 申请列表
        /// </summary>
        public List<long> RequestList { get; set; } = new();

        /// <summary>
        /// 黑名单
        /// </summary>
        public List<long> BlackList { get; set; } = new();
    }
}