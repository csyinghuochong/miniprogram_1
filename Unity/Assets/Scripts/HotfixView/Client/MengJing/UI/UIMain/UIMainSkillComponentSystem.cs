using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainSkillComponent))]
    [FriendOf(typeof(UIMainSkillComponent))]
    public static partial class UIMainSkillComponentSystem
    {
        [Invoke(TimerInvokeType.UIMainSkillTimer)]
        public class UIMainSkillTimer : ATimer<UIMainSkillComponent>
        {
            protected override void Run(UIMainSkillComponent self)
            {
                try
                {
                    self.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        [EntitySystem]
        private static void Awake(this UIMainSkillComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Button_AutoFight = rc.Get<GameObject>("Button_AutoFight").GetComponent<Button>();
            self.Transform_SkillList = rc.Get<GameObject>("Transform_SkillList").transform;
            self.UIMainSkillItem = rc.Get<GameObject>("UIMainSkillItem");
            self.UIMainSkillItem.SetActive(false);

            self.Button_AutoFight.AddListener(() => { self.OnAutoFight().Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIMainSkillComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.UIMainSkillItemList.Clear();
        }

        private static void Update(this UIMainSkillComponent self)
        {
            foreach (UIMainSkillItem item in self.UIMainSkillItemList)
            {
                item.UpdateCD();
            }
        }

        public static void BeforeEnterScene(this UIMainSkillComponent self, MapType mapType)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void AfterEnterScene(this UIMainSkillComponent self, MapType mapType)
        {
            List<(long, int)> showSkills = new();
            if (mapType == MapType.LocalLevel)
            {
                self.GameObject.SetActive(true);

                self.UpdateAutoFight(true);
                
                HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
                List<EntityRef<Unit>> allUnits = self.Root().CurrentScene().GetComponent<UnitComponent>().GetAll();
                foreach (Unit unit in allUnits)
                {
                    if (unit.Type == UnitType.Hero)
                    {
                        Hero hero = heroComponent.GetHero(unit.Id);
                        if (hero == null)
                        {
                            continue;
                        }

                        foreach (int skill in hero.Skills)
                        {
                            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skill);

                            if (skillConfig.SkillActType == SkillActType.Skill && skillConfig.SkillType == SkillType.Active)
                            {
                                showSkills.Add((unit.Id, skill));
                            }
                        }
                    }
                }

                while (self.UIMainSkillItemList.Count < showSkills.Count)
                {
                    GameObject go = UnityEngine.Object.Instantiate(self.UIMainSkillItem, self.Transform_SkillList);
                    UIMainSkillItem newItem = self.AddChild<UIMainSkillItem, GameObject>(go);
                    self.UIMainSkillItemList.Add(newItem);
                }

                for (int i = 0; i < showSkills.Count; i++)
                {
                    self.UIMainSkillItemList[i].UpdateInfo(showSkills[i].Item1, showSkills[i].Item2).Coroutine();
                    self.UIMainSkillItemList[i].GameObject.SetActive(true);
                }

                for (int i = showSkills.Count; i < self.UIMainSkillItemList.Count; i++)
                {
                    self.UIMainSkillItemList[i].GameObject.SetActive(false);
                }

                if (self.Timer == 0)
                {
                    self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.UIMainSkillTimer, self);
                }
            }
            else
            {
                self.GameObject.SetActive(false);
            }
        }

        private static async ETTask OnAutoFight(this UIMainSkillComponent self)
        {
            int error = await ClientLevelHelper.SetAutoFight(self.Root(), !self.AutoFight);
            
            if (error != ErrorCode.ERR_Success)
            {
                return;
            }
            
            self.UpdateAutoFight(!self.AutoFight);

            await ETTask.CompletedTask;
        }

        private static void UpdateAutoFight(this UIMainSkillComponent self, bool value)
        {
            self.AutoFight = value;
            self.Button_AutoFight.transform.Find("Image_On").gameObject.SetActive(self.AutoFight);
            self.Button_AutoFight.transform.Find("Image_Off").gameObject.SetActive(!self.AutoFight);
        }
    }
}