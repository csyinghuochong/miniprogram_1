using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ET.Client
{
    # region 事件监听

    // [Event(SceneType.Demo)]
    // public class OnUseSkill_ShowTip : AEvent<Scene, OnUseSkill>
    // {
    //     protected override async ETTask Run(Scene scene, OnUseSkill args)
    //     {
    //         Transform head = null;
    //         Unit unit = args.Unit;
    //
    //         SkillConfig skillConfig = SkillConfigCategory.Instance.Get(args.SkillConfigId);
    //         if (skillConfig.SkillActType == SkillActType.Normal)
    //         {
    //             return;
    //         }
    //
    //         if (unit.Type == UnitType.Monster)
    //         {
    //             UIMonsterHpComponent uiMonsterHpComponent = unit.GetComponent<UIMonsterHpComponent>();
    //             if (uiMonsterHpComponent == null)
    //             {
    //                 return;
    //             }
    //
    //             if (uiMonsterHpComponent.GameObject == null)
    //             {
    //                 return;
    //             }
    //
    //             head = uiMonsterHpComponent.GameObject.GetComponent<Transform>();
    //         }
    //
    //         if (unit.Type == UnitType.Hero)
    //         {
    //             UIHeroHpComponent uiHeroHpComponent = unit.GetComponent<UIHeroHpComponent>();
    //             if (uiHeroHpComponent == null)
    //             {
    //                 return;
    //             }
    //
    //             if (uiHeroHpComponent.GameObject == null)
    //             {
    //                 return;
    //             }
    //
    //             head = uiHeroHpComponent.GameObject.GetComponent<Transform>();
    //         }
    //
    //         if (head == null)
    //         {
    //             return;
    //         }
    //
    //         unit.Root().GetComponent<FloatingTextComponent>().ShowNormalText(skillConfig.SkillName, head);
    //
    //         await ETTask.CompletedTask;
    //     }
    // }

    [Event(SceneType.Demo)]
    public class StateChange_ShowTip : AEvent<Scene, StateChange>
    {
        protected override async ETTask Run(Scene scene, StateChange args)
        {
            Transform head = null;
            Unit unit = args.Unit;

            if (unit.Type == UnitType.Monster)
            {
                UIMonsterHpComponent uiMonsterHpComponent = unit.GetComponent<UIMonsterHpComponent>();
                if (uiMonsterHpComponent == null)
                {
                    return;
                }

                if (uiMonsterHpComponent.GameObject == null)
                {
                    return;
                }

                head = uiMonsterHpComponent.GameObject.GetComponent<Transform>();
            }

            if (unit.Type == UnitType.Hero)
            {
                UIHeroHpComponent uiHeroHpComponent = unit.GetComponent<UIHeroHpComponent>();
                if (uiHeroHpComponent == null)
                {
                    return;
                }

                if (uiHeroHpComponent.GameObject == null)
                {
                    return;
                }

                head = uiHeroHpComponent.GameObject.GetComponent<Transform>();
            }

            if (head == null)
            {
                return;
            }

            string name = "状态";
            if ((StateType)args.m2C_UnitStateUpdate.StateType == StateType.AllDamageImmune)
            {
                name = "无敌";
            }
            else if ((StateType)args.m2C_UnitStateUpdate.StateType == StateType.PhysicalImmune)
            {
                name = "免疫物理伤害";
            }
            else if ((StateType)args.m2C_UnitStateUpdate.StateType == StateType.MagicalImmune)
            {
                name = "免疫法术伤害";
            }
            else if ((StateType)args.m2C_UnitStateUpdate.StateType == StateType.Taunt)
            {
                name = "嘲讽";
            }
            else if ((StateType)args.m2C_UnitStateUpdate.StateType == StateType.Stun)
            {
                name = "眩晕";
            }
            else if ((StateType)args.m2C_UnitStateUpdate.StateType == StateType.Freeze)
            {
                name = "冰冻";
            }

            // 添加状态
            if (args.m2C_UnitStateUpdate.StateOperateType == 1)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowNormalText(ZString.Format("+{0}", name), head);
            }

            //移除状态
            if (args.m2C_UnitStateUpdate.StateOperateType == 2)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowNormalText(ZString.Format("-{0}", name), head);
            }

            await ETTask.CompletedTask;
        }
    }

    [NumericWatcher(SceneType.Current, NumericType.Now_Hp)]
    public class NumericWatcher_ShowDamageText : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            Transform head = null;

            if (unit.Type == UnitType.Monster)
            {
                UIMonsterHpComponent uiMonsterHpComponent = unit.GetComponent<UIMonsterHpComponent>();
                if (uiMonsterHpComponent == null)
                {
                    return;
                }

                if (uiMonsterHpComponent.GameObject == null)
                {
                    return;
                }

                head = uiMonsterHpComponent.GameObject.GetComponent<Transform>();
            }

            if (unit.Type == UnitType.Hero)
            {
                UIHeroHpComponent uiHeroHpComponent = unit.GetComponent<UIHeroHpComponent>();
                if (uiHeroHpComponent == null)
                {
                    return;
                }

                if (uiHeroHpComponent.GameObject == null)
                {
                    return;
                }

                head = uiHeroHpComponent.GameObject.GetComponent<Transform>();
            }

            if (head == null)
            {
                return;
            }

            switch (args.DamageType)
            {
                case DamageType.Physical:
                    unit.Root().GetComponent<FloatingTextComponent>().ShowPhysicalDamageText((args.OldValue - args.NewValue).ToString(), head);
                    break;
                case DamageType.Magical:
                    unit.Root().GetComponent<FloatingTextComponent>().ShowMagicDamageText((args.OldValue - args.NewValue).ToString(), head);
                    break;
                case DamageType.Critical:
                    unit.Root().GetComponent<FloatingTextComponent>().ShowCriDamageText((args.OldValue - args.NewValue).ToString(), head);
                    break;
                case DamageType.Recover:
                    unit.Root().GetComponent<FloatingTextComponent>().ShowRecoverText((args.NewValue - args.OldValue).ToString(), head);
                    break;
                case DamageType.Immune:
                    unit.Root().GetComponent<FloatingTextComponent>().ShowNormalText("免疫", head);
                    break;
            }
        }
    }

    #endregion

    [EntitySystemOf(typeof(FloatingTextComponent))]
    [FriendOf(typeof(FloatingTextComponent))]
    [FriendOf(typeof(FloatingText))]
    public static partial class FloatingTextComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FloatingTextComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this FloatingTextComponent self)
        {
            for (int i = self.FloatingTexts.Count - 1; i >= 0; i--)
            {
                FloatingText floatingText = self.FloatingTexts[i];
                floatingText.Update();
                if (floatingText.Time <= 0)
                {
                    floatingText.Dispose();
                    self.FloatingTexts.RemoveAt(i);
                }
            }
        }

        [EntitySystem]
        private static void Destroy(this FloatingTextComponent self)
        {
            self.FloatingTexts.Clear();
        }

        // 物理
        public static void ShowPhysicalDamageText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_PhysicalDamage.prefab";
            FloatingText floatingText = self.AddChild<FloatingText>();
            Vector3 offset = new Vector3(RandomHelper.RandomNumberFloat(0, 80), RandomHelper.RandomNumberFloat(0, 40), 0);
            floatingText.Init(text, 1.0f, path, offset, head);

            self.FloatingTexts.Add(floatingText);
        }

        // 魔法
        public static void ShowMagicDamageText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_MagicDamage.prefab";
            FloatingText floatingText = self.AddChild<FloatingText>();
            Vector3 offset = new Vector3(RandomHelper.RandomNumberFloat(-80, 0), RandomHelper.RandomNumberFloat(0,40), 0);
            floatingText.Init(text, 1.0f, path, offset, head);

            self.FloatingTexts.Add(floatingText);
        }

        // 暴击
        public static void ShowCriDamageText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_CriDamage.prefab";
            FloatingText floatingText = self.AddChild<FloatingText>();
            Vector3 offset = new Vector3(RandomHelper.RandomNumberFloat(-40, 40), RandomHelper.RandomNumberFloat(0,40), 0);
            floatingText.Init(text, 1.0f, path, offset, head);

            self.FloatingTexts.Add(floatingText);
        }

        // 恢复
        public static void ShowRecoverText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_Recover.prefab";
            FloatingText floatingText = self.AddChild<FloatingText>();
            Vector3 offset = new Vector3(RandomHelper.RandomNumberFloat(-40, 40), RandomHelper.RandomNumberFloat(0,40), 0);
            floatingText.Init(text, 1.0f, path, offset, head);

            self.FloatingTexts.Add(floatingText);
        }

        public static void ShowNormalText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_Normal.prefab";
            FloatingText floatingText = self.AddChild<FloatingText>();
            Vector3 offset = new Vector3(RandomHelper.RandomNumberFloat(-80, 80), RandomHelper.RandomNumberFloat(0,40), 0);
            floatingText.Init(text, 1.0f, path, offset, head);

            self.FloatingTexts.Add(floatingText);
        }

        // 提示
        public static void ShowTipText(this FloatingTextComponent self, string text)
        {
            string path = "Assets/Bundles/UI/Blood/Text_Tip.prefab";
            FloatingText floatingText = self.AddChild<FloatingText>();
            floatingText.Init(text, 1.0f, path, Vector3.zero);

            self.FloatingTexts.Add(floatingText);
        }
    }
}