using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class HeroComponentC : Entity, IAwake, IDestroy
    {
        public List<EntityRef<Hero>> Heros = new();

        public List<long> Formation { get; set; } = new();

        //最大上阵英雄数量
        public int maxTeamHeroCount { get; set; } = 5;

        //已上阵英雄数量
        public int currentTeamHeroCount { get; set; }
    }
}