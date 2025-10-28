using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(HeroComponentC))]
    [FriendOf(typeof(HeroComponentC))]
    public static partial class HeroComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this HeroComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this HeroComponentC self)
        {
            self.Heros.Clear();
            self.Heros = null;
        }

        public static Hero GetHero(this HeroComponentC self, long heroId)
        {
            self.Heros.TryGetValue(heroId, out EntityRef<Hero> hero);
            return hero;
        }

        public static void AddHeroFromMessage(this HeroComponentC self, HeroInfo heroInfo)
        {
            Hero hero = self.AddChildWithId<Hero>(heroInfo.Id);
            hero.FromMessage(heroInfo);
            self.Heros.Add(hero.Id, hero);

            EventSystem.Instance.Publish(self.Root(), new HeroUpdate());
        }

        public static void RemoveHeroById(this HeroComponentC self, long heroId)
        {
            if (!self.Heros.TryGetValue(heroId, out EntityRef<Hero> heroRef))
            {
                Log.Error($"HeroId:{heroId} not found");
                return;
            }

            Hero hero = heroRef;
            self.Heros.Remove(heroId);
            hero?.Dispose();

            EventSystem.Instance.Publish(self.Root(), new HeroUpdate());
        }

        public static void UpdateHero(this HeroComponentC self, HeroInfo heroInfo)
        {
            if (!self.Heros.TryGetValue(heroInfo.Id, out EntityRef<Hero> heroRef))
            {
                Log.Error($"HeroId:{heroInfo.Id} not found");
                return;
            }

            Hero hero = heroRef;
            hero.FromMessage(heroInfo);

            EventSystem.Instance.Publish(self.Root(), new HeroUpdate());
        }

        public static void Clear(this HeroComponentC self)
        {
            foreach (Hero hero in self.Heros.Values)
            {
                hero?.Dispose();
            }

            self.Heros.Clear();
        }

        public static List<Hero> GetAllHero(this HeroComponentC self)
        {
            List<Hero> heroes = new List<Hero>();
            foreach (Hero item in self.Heros.Values)
            {
                heroes.Add(item);
            }

            return heroes;
        }

        public static List<Hero> GetHerosByType(this HeroComponentC self, HeroType type)
        {
            List<Hero> Heros = new();
            foreach (Hero hero in self.Heros.Values)
            {
                HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);

                if (heroConfig.HeroType == (int)type)
                {
                    Heros.Add(hero);
                }
            }

            return Heros;
        }

        public static int GetAllHeroCount(this HeroComponentC self)
        {
            return self.Heros.Count;
        }

        public static long GetHeroIdByEquipmentId(this HeroComponentC self, long itemId)
        {
            foreach (Hero hero in self.Heros.Values)
            {
                foreach (long value in hero.Equipments.Values)
                {
                    if (value == itemId)
                    {
                        return hero.Id;
                    }
                }
            }

            return 0;
        }
    }
}