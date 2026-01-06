namespace ET.Server
{
    [EntitySystemOf(typeof(ArchiveComponentS))]
    [FriendOf(typeof(ArchiveComponentS))]
    public static partial class ArchiveComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this ArchiveComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ArchiveComponentS self)
        {
            self.ArchiveHeroList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this ArchiveComponentS self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is ArchiveHero archiveHero)
                {
                    self.ArchiveHeroList.Add(archiveHero);
                }
            }
        }

        public static void GetOrUpHero(this ArchiveComponentS self, Hero hero)
        {
            ArchiveHero UpdateArchiveHero = null;
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                if (archiveHero.HeroConfigId == hero.ConfigId)
                {
                    archiveHero.Lv = hero.Lv;
                    archiveHero.Star = hero.Star;

                    UpdateArchiveHero = archiveHero;
                    break;
                }
            }

            if (UpdateArchiveHero == null)
            {
                UpdateArchiveHero = self.AddChild<ArchiveHero>();
                UpdateArchiveHero.HeroConfigId = hero.ConfigId;
                UpdateArchiveHero.Lv = hero.Lv;
                UpdateArchiveHero.Star = hero.Star;
                self.ArchiveHeroList.Add(UpdateArchiveHero);
            }

            // 通知客户端
        }

        public static int GetCurrentScore(this ArchiveComponentS self)
        {
            int score = 0;
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                score += 10 + archiveHero.Lv * 1 + archiveHero.Star * 1;
            }

            return score;
        }
    }
}