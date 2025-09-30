using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(HeroComponentS))]
    [FriendOf(typeof(HeroComponentS))]
    public static partial class HeroComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this HeroComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this HeroComponentS self)
        {
            self.Heros.Clear();
            self.Heros = null;
        }

        [EntitySystem]
        private static void Deserialize(this HeroComponentS self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Hero hero)
                {
                    self.Heros.Add(hero.Id, hero);
                }
            }
        }

        public static Hero GetHero(this HeroComponentS self, long heroId)
        {
            self.Heros.TryGetValue(heroId, out EntityRef<Hero> hero);
            return hero;
        }

        public static void AddHeroByConfigId(this HeroComponentS self, int configId)
        {
            if (!HeroConfigCategory.Instance.Contain(configId))
            {
                return;
            }

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(configId);

            Hero hero = self.AddChild<Hero>();
            hero.ConfigId = configId;
            hero.Lv = 1;
            self.AddHero(hero);
        }

        public static void AddHero(this HeroComponentS self, Hero hero)
        {
            if (hero.Parent != self)
            {
                self.AddChild(hero);
            }

            if (self.Heros.ContainsKey(hero.Id))
            {
                return;
            }

            self.Heros.Add(hero.Id, hero);
            HeroNoticeHelper.SyncHeroInfo(self.GetParent<Unit>(), hero, HeroOpType.Add);
        }

        // 直接消耗掉
        public static bool RemoveHero(this HeroComponentS self, long heroId)
        {
            if (!self.Heros.TryGetValue(heroId, out EntityRef<Hero> heroRef))
            {
                return false;
            }

            Hero hero = heroRef;
            self.Heros.Remove(heroId);
            HeroNoticeHelper.SyncHeroInfo(self.GetParent<Unit>(), hero, HeroOpType.Remove);
            hero?.Dispose();

            return true;
        }

        public static List<Hero> GetAllHeros(this HeroComponentS self)
        {
            List<Hero> heros = new List<Hero>();
            foreach (Hero hero in self.Heros.Values)
            {
                heros.Add(hero);
            }

            return heros;
        }
    }
}