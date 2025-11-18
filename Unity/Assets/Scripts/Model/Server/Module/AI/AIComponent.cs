using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    // 客户端挂在ClientScene上，服务端挂在Unit上
    //[ComponentOf(typeof(Scene))]
    [ComponentOf(typeof(Unit))]
    public class AIComponent : Entity, IAwake<int>, IDestroy
    {
        public int AIConfigId { get; set; }
        public ETCancellationToken CancellationToken;
        public long Timer;
        public int Current;
        public long TargetId { get; set; } //攻击目标
        public long BeAttackId{ get; set; } //被谁打了
        public float FollowDistance { get; set; } //跟随距离，超过这个距离要马上跟上主人
        public bool IsRetreat { get; set; }
        public float ActDistance { get; set; } //攻击距离
        public List<int> AISkillIDList { get; set; } = new(); //当前所有技能
        public float3 TargetZhuiJi { get; set; }
        public MapType MapType { get; set; }
        public long AIDelay;
        public List<float3> TargetPoint { get; set; } = new();
    }
}