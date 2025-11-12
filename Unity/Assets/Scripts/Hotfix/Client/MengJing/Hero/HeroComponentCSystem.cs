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
            foreach (Hero hero in self.Heros)
            {
                if (hero.Id == heroId)
                {
                    return hero;
                }
            }
            return null;
        }

        public static void AddHeroFromMessage(this HeroComponentC self, HeroInfo heroInfo)
        {
            Hero hero = self.AddChildWithId<Hero>(heroInfo.Id);
            hero.FromMessage(heroInfo);
            self.Heros.Add(hero);

            EventSystem.Instance.Publish(self.Root(), new HeroUpdate());
        }

        public static void RemoveHeroById(this HeroComponentC self, long heroId)
        {
            Hero heroToRemove = null;
            foreach (Hero hero in self.Heros)
            {
                if (hero.Id == heroId)
                {
                    heroToRemove = hero;
                    break;
                }
            }

            if (heroToRemove == null)
            {
                Log.Error($"HeroId:{heroId} not found");
                return;
            }

            self.Heros.Remove(heroToRemove);
            heroToRemove?.Dispose();

            EventSystem.Instance.Publish(self.Root(), new HeroUpdate());
        }

        public static void UpdateHero(this HeroComponentC self, HeroInfo heroInfo)
        {
            Hero targetHero = null;
            foreach (Hero hero in self.Heros)
            {
                if (hero.Id == heroInfo.Id)
                {
                    targetHero = hero;
                    break;
                }
            }

            if (targetHero == null)
            {
                Log.Error($"HeroId:{heroInfo.Id} not found");
                return;
            }

            targetHero.FromMessage(heroInfo);

            EventSystem.Instance.Publish(self.Root(), new HeroUpdate());
        }

        public static void Clear(this HeroComponentC self)
        {
            foreach (Hero hero in self.Heros)
            {
                hero?.Dispose();
            }

            self.Heros.Clear();
        }

        public static List<Hero> GetAllHero(this HeroComponentC self)
        {
            List<Hero> heroes = new List<Hero>();
            foreach (Hero item in self.Heros)
            {
                heroes.Add(item);
            }

            return heroes;
        }

        public static List<Hero> GetHerosByType(this HeroComponentC self, HeroType type)
        {
            List<Hero> Heros = new();
            foreach (Hero hero in self.Heros)
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
            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (Hero hero in self.Heros)
            {
                HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);

                dic.TryAdd(hero.ConfigId, 1);
            }

            return dic.Count;
        }

        public static long GetHeroIdByEquipmentId(this HeroComponentC self, long itemId)
        {
            foreach (Hero hero in self.Heros)
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