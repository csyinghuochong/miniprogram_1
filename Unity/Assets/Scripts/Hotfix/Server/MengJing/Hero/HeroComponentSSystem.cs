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
                    HeroHelper.UpdateHeroStats(self.GetParent<Unit>(), hero);
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
            if (!HeroConfigCategory.Instance.DataMap.ContainsKey(configId))
            {
                return;
            }

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(configId);

            Hero hero = self.AddChild<Hero>();
            hero.ConfigId = configId;
            hero.Lv = 1;

            HeroHelper.UpdateHeroStats(self.GetParent<Unit>(), hero);
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
            HeroHelper.SyncHeroInfo(self.GetParent<Unit>(), hero, HeroOpType.Add);
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
            HeroHelper.SyncHeroInfo(self.GetParent<Unit>(), hero, HeroOpType.Remove);
            hero?.Dispose();

            return true;
        }

        public static List<Hero> GetAllHero(this HeroComponentS self)
        {
            List<Hero> heros = new List<Hero>();
            foreach (Hero hero in self.Heros.Values)
            {
                heros.Add(hero);
            }

            return heros;
        }

        public static void OnLogin(this HeroComponentS self)
        {
            if (self.Formation.Count == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    self.Formation.Add(0);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="opType">0上阵 1下阵</param>
        /// <param name="heroId"></param>
        /// <param name="slotIndex"></param>
        /// <returns></returns>
        public static int SetFormation(this HeroComponentS self, int opType, long heroId, int slotIndex)
        {
            Hero hero = self.GetHero(heroId);

            if (hero == null)
            {
                return ErrorCode.ERR_ModifyData;
            }

            if (slotIndex < 1 || slotIndex > 9)
            {
                return ErrorCode.ERR_ModifyData;
            }

            if (opType == 0)
            {
                for (int i = 0; i < self.Formation.Count; i++)
                {
                    if (self.Formation[i] == heroId)
                    {
                        self.Formation[i] = self.Formation[slotIndex - 1];
                    }
                }

                self.Formation[slotIndex - 1] = heroId;
            }
            else
            {
                self.Formation[slotIndex - 1] = 0;
            }

            return ErrorCode.ERR_Success;
        }
    }
}