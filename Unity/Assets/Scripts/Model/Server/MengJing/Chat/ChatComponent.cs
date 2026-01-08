using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(ChatUnit))]
    public class ChatComponent : Entity, IAwake, IDestroy, IDeserialize
    {
        public List<string> ChatRoomKeyList { get; set; } = new();

        public long UnmuteTime { get; set; } //解除禁言时间
        public List<long> ReportList { get; set; } = new(); //举报列表
    }
}