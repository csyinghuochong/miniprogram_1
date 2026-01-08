namespace ET.Server
{
    [EntitySystemOf(typeof(ArchiveComponent))]
    [FriendOf(typeof(ArchiveComponent))]
    public static partial class ArchiveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ArchiveComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ArchiveComponent self)
        {
            self.ArchiveHeroList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this ArchiveComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is ArchiveHero archiveHero)
                {
                    self.ArchiveHeroList.Add(archiveHero);
                }
            }
        }

        public static void ActiveArchiveHero(this ArchiveComponent self, Hero hero)
        {
            bool change = false;
            ArchiveHero UpdateArchiveHero = null;
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                if (archiveHero.HeroConfigId == hero.ConfigId)
                {
                    if (hero.Star > archiveHero.Star)
                    {
                        archiveHero.Star = hero.Star;
                        change = true;
                    }

                    UpdateArchiveHero = archiveHero;
                    break;
                }
            }

            if (UpdateArchiveHero == null)
            {
                UpdateArchiveHero = self.AddChild<ArchiveHero>();
                UpdateArchiveHero.HeroConfigId = hero.ConfigId;
                UpdateArchiveHero.Star = hero.Star;
                self.ArchiveHeroList.Add(UpdateArchiveHero);

                change = true;
            }

            if (change)
            {
                M2C_ArchiveHeroUpdate message = M2C_ArchiveHeroUpdate.Create();
                message.ArchiveHeroInfo = UpdateArchiveHero.ToMessage();
                MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
            }
        }

        public static int GetCurrentPoint(this ArchiveComponent self)
        {
            int point = 0;
            foreach (ArchiveHero archiveHero in self.ArchiveHeroList)
            {
                point += ConfigData.ArchiveHeroAddPoint + archiveHero.Star * ConfigData.ArchiveHeroStarAddPoint;
            }

            return point;
        }
    }
}