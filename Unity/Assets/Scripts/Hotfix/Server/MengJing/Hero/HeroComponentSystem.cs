using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(HeroComponent))]
    [FriendOf(typeof(HeroComponent))]
    public static partial class HeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this HeroComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this HeroComponent self)
        {
            self.Heros.Clear();
            self.Heros = null;
        }

        [EntitySystem]
        private static void Deserialize(this HeroComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Hero hero)
                {
                    self.Heros.Add(hero);
                }
            }
        }

        public static Hero GetHero(this HeroComponent self, long heroId)
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

        public static Hero GetHeroByConfigId(this HeroComponent self, int heroConfigId)
        {
            foreach (Hero hero in self.Heros)
            {
                if (hero.ConfigId == heroConfigId)
                {
                    return hero;
                }
            }

            return null;
        }

        public static void AddHeroByConfigId(this HeroComponent self, int configId)
        {
            if (!HeroConfigCategory.Instance.DataMap.ContainsKey(configId))
            {
                return;
            }

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(configId);

            Hero hero = self.AddChild<Hero>();
            hero.ConfigId = configId;
            hero.Lv = 1;

            self.RefreshHero(hero);

            self.AddHero(hero);
        }

        private static void RefreshHero(this HeroComponent self, Hero hero)
        {
            hero.Equipments.TryAdd((int)EquipSlotType.Toukui, 0);
            hero.Equipments.TryAdd((int)EquipSlotType.Yifu, 0);
            hero.Equipments.TryAdd((int)EquipSlotType.Kuzi, 0);
            hero.Equipments.TryAdd((int)EquipSlotType.Xiezi, 0);
            hero.Equipments.TryAdd((int)EquipSlotType.Xianglian, 0);
            hero.Equipments.TryAdd((int)EquipSlotType.Wuqi, 0);
                
            HeroHelper.UpdateHeroSkill(hero);
            HeroHelper.UpdateHeroNumeric(self.GetParent<Unit>(), hero);
        }

        public static void AddHero(this HeroComponent self, Hero hero)
        {
            if (hero.Parent != self)
            {
                self.AddChild(hero);
            }

            if (self.Heros.Contains(hero))
            {
                return;
            }

            self.Heros.Add(hero);
            
            Unit unit = self.GetParent<Unit>();

            EventSystem.Instance.Publish(self.Scene(), new TriggerAchievement()
            {
                Unit = self.GetParent<Unit>(), TargetType = AchievementTargetType.HaveHeroId
            });
            EventSystem.Instance.Publish(self.Scene(), new TriggerAchievement()
            {
                Unit = self.GetParent<Unit>(), TargetType = AchievementTargetType.HaveHeroValue
            });
            
            HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Add);
        }

        // 直接消耗掉
        public static bool RemoveHero(this HeroComponent self, long heroId)
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
                return false;
            }

            self.Heros.Remove(heroToRemove);
            HeroHelper.SyncHeroInfo(self.GetParent<Unit>(), heroToRemove, HeroOpType.Remove);
            heroToRemove?.Dispose();

            return true;
        }

        public static List<Hero> GetAllHero(this HeroComponent self)
        {
            List<Hero> heros = new List<Hero>();
            foreach (Hero hero in self.Heros)
            {
                heros.Add(hero);
            }

            return heros;
        }

        public static void OnLogin(this HeroComponent self)
        {
            if (self.Formation.Count == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    self.Formation.Add(0);
                }
            }

            foreach (Hero hero in self.Heros)
            {
                self.RefreshHero(hero);
            }
        }

        /// <summary>
        /// 设置英雄阵容
        /// </summary>
        /// <param name="self"></param>
        /// <param name="opType">0上阵 1下阵</param>
        /// <param name="heroId"></param>
        /// <param name="slotIndex"></param>
        /// <returns></returns>
        public static int SetFormation(this HeroComponent self, int opType, long heroId, int slotIndex)
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

            Unit unit = self.GetParent<Unit>();
            EventSystem.Instance.Publish(unit.Scene(), new UpdateTotalCombatPower() { Unit = unit });
            
            return ErrorCode.ERR_Success;
        }

        public static long GetHeroIdFromFormation(this HeroComponent self, int index)
        {
            if (index < 1 || index > self.Formation.Count)
            {
                return 0;
            }

            return self.Formation[index - 1];
        }

        public static Hero GetFirstHero(this HeroComponent self)
        {
            foreach (long heroId in self.Formation)
            {
                if (heroId != 0)
                {
                    return self.GetHero(heroId);
                }
            }

            return null;
        }

        public static float3 GetHeroPosition(this HeroComponent self, long heroId)
        {
            for (int i = 0; i < self.Formation.Count; i++)
            {
                if (self.Formation[i] != heroId)
                {
                    continue;
                }

                float3 position = i switch
                {
                    0 => new float3(-3, 3, 0),
                    1 => new float3(0, 3, 0),
                    2 => new float3(3, 3, 0),
                    3 => new float3(-3, 0, 0),
                    4 => new float3(0, 0, 0),
                    5 => new float3(3, 0, 0),
                    6 => new float3(-3, -3, 0),
                    7 => new float3(0, -3, 0),
                    8 => new float3(3, -3, 0),
                    _ => float3.zero
                };

                return position;
            }

            return float3.zero;
        }
    }
}