namespace ET.Client
{
    [EntitySystemOf(typeof(ArchiveComponentC))]
    public static partial class ArchiveComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this ArchiveComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ArchiveComponentC self)
        {
        }

        public static void Clear(this ArchiveComponentC self)
        {
            self.ReceivedArchiveRewardIds.Clear();
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                archiveHero.Dispose();
            }

            self.ArchiveHeroList.Clear();
        }

        public static void AddOrUpdateArchiveHero(this ArchiveComponentC self, ArchiveHeroInfo archiveHeroInfo)
        {
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                if (archiveHero.HeroConfigId == archiveHeroInfo.HeroConfigId)
                {
                    archiveHero.FromMessage(archiveHeroInfo);
                    return;
                }
            }

            ArchiveHero newArchiveHero = self.AddChild<ArchiveHero>();
            newArchiveHero.FromMessage(archiveHeroInfo);
            self.ArchiveHeroList.Add(newArchiveHero);
        }

        public static int GetCurrentScore(this ArchiveComponentC self)
        {
            int score = 0;
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                score += ConfigData.ArchiveHeroAddScore + archiveHero.Star * ConfigData.ArchiveHeroStarAddScore;
            }

            return score;
        }

        public static ArchiveHero GetArchiveHero(this ArchiveComponentC self, int heroConfigId)
        {
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                if (archiveHero.HeroConfigId == heroConfigId)
                {
                    return archiveHero;
                }
            }

            return null;
        }
    }
}