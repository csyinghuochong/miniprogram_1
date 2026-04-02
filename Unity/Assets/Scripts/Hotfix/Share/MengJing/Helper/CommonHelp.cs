using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    public static class CommonHelp
    {
        public const int MaxZone = 1024;

        public static int GetCenterZone()
        {
            return 1000;
        }

        public static bool IsRobotZone(int zone)
        {
            return zone == 1001;
        }

        //版号专区
        public static bool IsBanHaoZone(int zone)
        {
            return zone == 1011;
        }

        //内部专区
        public static bool IsAlphaZone(int zone)
        {
            return zone == 1013;
        }

        public const int Version = 20240130;

        //public static string LocalIp = "192.168.1.16"; 
        public const string LocalIp = "127.0.0.1";

        public const bool AccountOldLogic = true;

        public static int GetMaxBaoShiDu()
        {
            return 120;
        }

        public static int GetSkillCdRate(int sceneType)
        {
            return 1;
        }

        public static int GetDayByTime(long time)
        {
            DateTime dateTime = TimeInfo.Instance.ToDateTime(time);
            return dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;
        }

        // 根据配置计算英雄属性
        public static Dictionary<int, long> CalculateHeroNumericByConfig(int heroConfigId, int lv, int star)
        {
            Dictionary<int, long> numericDic = new Dictionary<int, long>();

            // 英雄配置表属性
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(heroConfigId);
            long base_MaxHp = heroConfig.BaseHp;
            long base_MinAct = heroConfig.BaseAct;
            long base_MaxAct = heroConfig.BaseAct;
            long base_MageAct = heroConfig.BaseMage;
            long base_MinDef = heroConfig.BaseDef;
            long base_MaxDef = heroConfig.BaseDef;
            long base_MinAdf = heroConfig.BaseAdf;
            long base_MaxAdf = heroConfig.BaseAdf;
            long base_Cri = heroConfig.BaseCri;
            long base_ReCri = heroConfig.BaseReCri;
            long base_Eva = heroConfig.BaseEva;
            long base_Hit = heroConfig.BaseHit;
            long base_HitLess = heroConfig.BaseHitLess;
            long base_MoveSpeed = heroConfig.BaseMoveSpeed;
            long base_AtkSpeed = heroConfig.BaseAtkSpeed;
            // long base_Combo = 0;
            // long base_Counterattack = 0;
            // long base_LifeSteal = 0;
            // long base_ReCombo = 0;
            // long base_ReCounterattack = 0;
            // long base_ReLifeSteal = 0;
            // long base_ReEva = 0;
            long combatPower = 0;

            // 等级成长
            base_MaxHp += lv * heroConfig.LvHp;
            base_MinAct += lv * heroConfig.LvAct;
            base_MaxAct += lv * heroConfig.LvAct;
            base_MinDef += lv * heroConfig.LvDef;
            base_MaxDef += lv * heroConfig.LvDef;
            base_MinAdf += lv * heroConfig.LvAdf;
            base_MaxAdf += lv * heroConfig.LvAdf;

            // 星级
            base_MaxHp += star * 100;
            base_MinAct += star * 100;
            base_MaxAct += star * 100;
            base_MinDef += star * 100;
            base_MaxDef += star * 100;
            base_MinAdf += star * 100;
            base_MaxAdf += star * 100;

            // 计算战斗力
            combatPower = base_MaxHp + base_MinAct + base_MaxAct + base_MinDef + base_MaxDef + base_MinAdf + base_MaxAdf;

            // 保存数据
            numericDic.Add(NumericType.Now_Hp, base_MaxHp);
            numericDic.Add(NumericType.Base_MaxHp_Base, base_MaxHp);
            numericDic.Add(NumericType.Base_MinAct_Base, base_MinAct);
            numericDic.Add(NumericType.Base_MaxAct_Base, base_MaxAct);
            numericDic.Add(NumericType.Base_Mage_Base, base_MageAct);
            numericDic.Add(NumericType.Base_MinDef_Base, base_MinDef);
            numericDic.Add(NumericType.Base_MaxDef_Base, base_MaxDef);
            numericDic.Add(NumericType.Base_MinAdf_Base, base_MaxAdf);
            numericDic.Add(NumericType.Base_MaxAdf_Base, base_MinAdf);
            numericDic.Add(NumericType.Base_Cri_Base, base_Cri);
            numericDic.Add(NumericType.Base_ReCri_Base, base_ReCri);
            numericDic.Add(NumericType.Base_Eva_Base, base_Eva);
            numericDic.Add(NumericType.Base_Hit_Base, base_Hit);
            numericDic.Add(NumericType.Base_HitDamageLessPro_Base, base_HitLess);
            numericDic.Add(NumericType.Base_Speed_Base, base_MoveSpeed);
            numericDic.Add(NumericType.Base_AtkSpeed_Base, base_AtkSpeed);
            numericDic.Add(NumericType.CombatPower, combatPower);

            return numericDic;
        }

        // 计算英雄属性、战斗力
        public static Dictionary<int, long> CalculateHeroNumeric(Hero hero, List<Item> equipments)
        {
            Dictionary<int, long> numericDic = new Dictionary<int, long>();

            // 英雄配置表属性
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            long base_MaxHp = heroConfig.BaseHp;
            long base_MinAct = heroConfig.BaseAct;
            long base_MaxAct = heroConfig.BaseAct;
            long base_MageAct = heroConfig.BaseMage;
            long base_MinDef = heroConfig.BaseDef;
            long base_MaxDef = heroConfig.BaseDef;
            long base_MinAdf = heroConfig.BaseAdf;
            long base_MaxAdf = heroConfig.BaseAdf;
            long base_Cri = heroConfig.BaseCri;
            long base_ReCri = heroConfig.BaseReCri;
            long base_Eva = heroConfig.BaseEva;
            long base_Hit = heroConfig.BaseHit;
            long base_HitLess = heroConfig.BaseHitLess;
            long base_MoveSpeed = heroConfig.BaseMoveSpeed;
            long base_AtkSpeed = heroConfig.BaseAtkSpeed;
            long base_MaxAnger = heroConfig.MaxAnger;
            long base_Combo = 0;
            long base_Counterattack = 0;
            long base_LifeSteal = 0;
            long base_ReCombo = 0;
            long base_ReCounterattack = 0;
            long base_ReLifeSteal = 0;
            long base_ReEva = 0;
            long combatPower = 0;

            // 等级成长
            int lv = hero.Lv;
            base_MaxHp += lv * heroConfig.LvHp;
            base_MinAct += lv * heroConfig.LvAct;
            base_MaxAct += lv * heroConfig.LvAct;
            base_MinDef += lv * heroConfig.LvDef;
            base_MaxDef += lv * heroConfig.LvDef;
            base_MinAdf += lv * heroConfig.LvAdf;
            base_MaxAdf += lv * heroConfig.LvAdf;

            // 星级
            int star = hero.Star;
            base_MaxHp += star * 100;
            base_MinAct += star * 100;
            base_MaxAct += star * 100;
            base_MinDef += star * 100;
            base_MaxDef += star * 100;
            base_MinAdf += star * 100;
            base_MaxAdf += star * 100;

            // 装备
            foreach (Item item in equipments)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
                EquipConfig equipConfig = EquipConfigCategory.Instance.Get(itemConfig.ItemEquipID);

                // 装备配置属性
                base_MinAct += equipConfig.EquipMinAct;
                base_MaxAct += equipConfig.EquipMaxAct;
                base_MinDef += equipConfig.EquipMinDef;
                base_MaxDef += equipConfig.EquipMaxDef;
                base_MinAdf += equipConfig.EquipMinAdf;
                base_MaxAdf += equipConfig.EquipMaxAdf;
                base_MaxHp += equipConfig.EquipHp;
                base_AtkSpeed += equipConfig.EquipAtkSpeed;
                base_MoveSpeed += equipConfig.EquipMoveSpeed;
                base_Cri += equipConfig.EquipCri;
                base_Combo += equipConfig.EquipCombo;
                base_Counterattack += equipConfig.EquipCounterattack;
                base_LifeSteal += equipConfig.EquipLifeSteal;
                base_Eva += equipConfig.EquipEva;
                base_ReCri += equipConfig.EquipReCri;
                base_ReCombo += equipConfig.EquipReCombo;
                base_ReCounterattack += equipConfig.EquipReCounterattack;
                base_ReLifeSteal += equipConfig.EquipLifeSteal;
                base_ReEva += equipConfig.EquipReEva;

                // TODO 装备词条属性
            }

            // 计算战斗力
            combatPower = base_MaxHp + base_MinAct + base_MaxAct + base_MinDef + base_MaxDef + base_MinAdf + base_MaxAdf;

            // 保存数据
            numericDic.Add(NumericType.Now_Hp, base_MaxHp);
            numericDic.Add(NumericType.Base_MaxHp_Base, base_MaxHp);
            numericDic.Add(NumericType.Base_MinAct_Base, base_MinAct);
            numericDic.Add(NumericType.Base_MaxAct_Base, base_MaxAct);
            numericDic.Add(NumericType.Base_Mage_Base, base_MageAct);
            numericDic.Add(NumericType.Base_MinDef_Base, base_MinDef);
            numericDic.Add(NumericType.Base_MaxDef_Base, base_MaxDef);
            numericDic.Add(NumericType.Base_MinAdf_Base, base_MaxAdf);
            numericDic.Add(NumericType.Base_MaxAdf_Base, base_MinAdf);
            numericDic.Add(NumericType.Base_Cri_Base, base_Cri);
            numericDic.Add(NumericType.Base_ReCri_Base, base_ReCri);
            numericDic.Add(NumericType.Base_Eva_Base, base_Eva);
            numericDic.Add(NumericType.Base_Hit_Base, base_Hit);
            numericDic.Add(NumericType.Base_HitDamageLessPro_Base, base_HitLess);
            numericDic.Add(NumericType.Base_Speed_Base, base_MoveSpeed);
            numericDic.Add(NumericType.Base_AtkSpeed_Base, base_AtkSpeed);
            numericDic.Add(NumericType.CombatPower, combatPower);
            numericDic.Add(NumericType.Base_MaxAngerValue_Base, base_MaxAnger);

            return numericDic;
        }

        /// <summary>
        /// 返回一个可以装备此类型道具的孔位
        /// </summary>
        /// <param name="equipments"></param>
        /// <param name="itemSubType"></param>
        /// <returns></returns>
        public static EquipSlotType GetCanEquipSlot(Dictionary<int, long> equipments, ItemSubType itemSubType)
        {
            EquipSlotType equipSlotType = EquipSlotType.None;
            switch (itemSubType)
            {
                case ItemSubType.Toukui:
                    equipSlotType = EquipSlotType.Toukui;
                    break;
                case ItemSubType.Yifu:
                    equipSlotType = EquipSlotType.Yifu;
                    break;
                case ItemSubType.Kuzi:
                    equipSlotType = EquipSlotType.Kuzi;
                    break;
                case ItemSubType.Xiezi:
                    equipSlotType = EquipSlotType.Xiezi;
                    break;
                case ItemSubType.Xianglian:
                    equipSlotType = EquipSlotType.Xianglian;
                    break;
                case ItemSubType.Wuqi:
                    equipSlotType = EquipSlotType.Wuqi;

                    // 如果有几个孔位都可以装备同一种装备，比如有2个武器孔位，3个宝石孔位
                    // 有空位置放空位，没有就放最后一个
                    // if (equipments.ContainsKey((int)EquipSlotType.Wuqi))
                    // {
                    //     if (equipments[(int)EquipSlotType.Wuqi] == 0)
                    //     {
                    //         equipSlotType = EquipSlotType.Wuqi;
                    //         break;
                    //     }
                    //
                    //     equipSlotType = EquipSlotType.Wuqi;
                    // }
                    //
                    // if (equipments.ContainsKey((int)EquipSlotType.Wuqi_2))
                    // {
                    //     if (equipments[(int)EquipSlotType.Wuqi_2] == 0)
                    //     {
                    //         equipSlotType = EquipSlotType.Wuqi_2;
                    //         break;
                    //     }
                    //     
                    //     equipSlotType = EquipSlotType.Wuqi_2;
                    // }

                    break;
            }

            if (!equipments.ContainsKey((int)equipSlotType))
            {
                equipSlotType = EquipSlotType.None;
            }

            return equipSlotType;
        }

        public static string GetMapObjName(MapType mapType)
        {
            string mapObjName = "";
            switch (mapType)
            {
                case MapType.Init:
                    mapObjName = "Init";
                    break;
                case MapType.Login:
                    mapObjName = "Login";
                    break;
                case MapType.MainCity:
                    mapObjName = "MainCity";
                    break;
                case MapType.LocalLevel:
                    mapObjName = "Level";
                    break;
                default:
                    break;
            }

            return mapObjName;
        }

        public static string GetChatRoomKey(long userId1, long userId2)
        {
            long minUserId = Math.Min(userId1, userId2);
            long maxUserId = Math.Max(userId1, userId2);

            return $"{minUserId}_{maxUserId}";
        }

        public static List<RewardItem> GetRecycleItems(List<EntityRef<Item>> itemList)
        {
            List<RewardItem> rewardItemList = new List<RewardItem>();

            foreach (Item item in itemList)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                foreach (RewardItem rewardItem in itemConfig.RecycleItem)
                {
                    bool isExist = false;
                    for (int i = 0; i < rewardItemList.Count; i++)
                    {
                        RewardItem old = rewardItemList[i];

                        if (old.ItemId == rewardItem.ItemId)
                        {
                            isExist = true;
                            old.ItemNum += rewardItem.ItemNum * item.Num;
                            rewardItemList[i] = old;
                        }
                    }

                    if (!isExist)
                    {
                        rewardItemList.Add(new RewardItem() { ItemId = rewardItem.ItemId, ItemNum = rewardItem.ItemNum * item.Num });
                    }
                }
            }

            return rewardItemList;
        }

        public static List<RewardItem> GetRecycleItems(List<EntityRef<Hero>> heroList)
        {
            List<RewardItem> rewardItemList = new List<RewardItem>();

            foreach (Hero hero in heroList)
            {
                HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);

                foreach (RewardItem rewardItem in heroConfig.RecycleItem)
                {
                    bool isExist = false;
                    for (int i = 0; i < rewardItemList.Count; i++)
                    {
                        RewardItem old = rewardItemList[i];

                        if (old.ItemId == rewardItem.ItemId)
                        {
                            isExist = true;
                            old.ItemNum += rewardItem.ItemNum;
                            rewardItemList[i] = old;
                        }
                    }

                    if (!isExist)
                    {
                        rewardItemList.Add(rewardItem);
                    }
                }
            }

            return rewardItemList;
        }
    }
}