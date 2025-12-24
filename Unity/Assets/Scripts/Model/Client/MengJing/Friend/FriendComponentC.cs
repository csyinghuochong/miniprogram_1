using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class FriendComponentC : Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 好友列表
        /// </summary>
        public List<EntityRef<FriendDate>> FriendList { get; set; } = new();

        /// <summary>
        /// 申请列表
        /// </summary>
        public List<EntityRef<FriendDate>> RequestList { get; set; } = new();

        /// <summary>
        /// 黑名单
        /// </summary>
        public List<EntityRef<FriendDate>> BlackList { get; set; } = new();
    }
}