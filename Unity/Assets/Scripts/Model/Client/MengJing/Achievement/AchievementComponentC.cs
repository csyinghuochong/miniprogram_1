using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class AchievementComponentC : Entity, IAwake, IDestroy
    {
        public List<int> ReceivedAchievementRewardIds { get; set; } = new();

        public List<EntityRef<Achievement>> AchievementList { get; set; } = new();
    }
}