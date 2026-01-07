using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ArchiveComponentC : Entity, IAwake, IDestroy
    {
        // 已领取的图鉴奖励ID列表
        public List<int> ReceivedArchiveRewardIds { get; set; } = new();

        public List<EntityRef<ArchiveHero>> ArchiveHeroList { get; set; } = new();
    }
}