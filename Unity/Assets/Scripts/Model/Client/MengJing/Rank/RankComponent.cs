using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class RankComponent : Entity, IAwake, IDestroy
    {
        public List<EntityRef<RankData>> PlayerRankDataList { get; set; } = new();
    }
}