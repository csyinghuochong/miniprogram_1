namespace ET
{
    [EntitySystemOf(typeof(ArchiveHero))]
    [FriendOf(typeof(ArchiveHero))]
    public static partial class ArchiveHeroSystem
    {
        [EntitySystem]
        private static void Awake(this ArchiveHero self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ArchiveHero self)
        {
        }

        public static ArchiveHeroInfo ToMessage(this ArchiveHero self)
        {
            ArchiveHeroInfo archiveHeroInfo = ArchiveHeroInfo.Create();
            archiveHeroInfo.HeroConfigId = self.HeroConfigId;
            archiveHeroInfo.Lv = self.Lv;
            archiveHeroInfo.Star = self.Star;
            return archiveHeroInfo;
        }

        public static void FromMessage(this ArchiveHero self, ArchiveHeroInfo archiveHeroInfo)
        {
            self.HeroConfigId = archiveHeroInfo.HeroConfigId;
            self.Lv = archiveHeroInfo.Lv;
            self.Star = archiveHeroInfo.Star;
        }
    }
}