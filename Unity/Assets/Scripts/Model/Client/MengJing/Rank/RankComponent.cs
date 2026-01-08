using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class RankComponent : Entity, IAwake, IDestroy
    {
        public List<EntityRef<PlayerCombatPowerRank>> PlayerCombatPowerRankList = new();

        public List<EntityRef<AllianceRank>> AllianceRankList { get; set; } = new();
    }
}