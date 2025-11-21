using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ET.Client
{
    # region 事件监听

    [Event(SceneType.Demo)]
    public class OnUseSkill_ShowTip : AEvent<Scene, OnUseSkill>
    {
        protected override async ETTask Run(Scene scene, OnUseSkill args)
        {
            Transform head = null;
            Unit unit = args.Unit;

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(args.SkillConfigId);
            if (skillConfig.SkillActType == SkillActType.Normal)
            {
                return;
            }
            
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

            unit.Root().GetComponent<FloatingTextComponent>().ShowNormalText(skillConfig.SkillName, head);

            await ETTask.CompletedTask;
        }
    }

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

    # endregion
    
    [EntitySystemOf(typeof(FloatingTextComponent))]
    [FriendOf(typeof(FloatingTextComponent))]
    public static partial class FloatingTextComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FloatingTextComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FloatingTextComponent self)
        {
        }

        public static void ShowPhysicalDamageText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_PhysicalDamage.prefab";
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(path, self.InstanceId, true,
                (gameObject, instanceId) =>
                {
                    if (instanceId != self.InstanceId)
                    {
                        if (gameObject != null)
                        {
                            UnityEngine.Object.DestroyImmediate(gameObject);
                        }

                        return;
                    }

                    gameObject.transform.SetParent(self.Root().GetComponent<GlobalComponent>().BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().SetText(text);
                    }

                    gameObject.transform.position = head.position;

                    if (textTransform != null)
                    {
                        textTransform.localPosition = Vector3.zero;
                        Sequence seq = DOTween.Sequence();
                        seq.Append(textTransform.DOLocalMoveY(100f, 1.0f).SetEase(Ease.OutQuad))
                                .OnUpdate(() =>
                                {
                                    if (gameObject != null && head != null)
                                    {
                                        gameObject.transform.position = head.position;
                                    }
                                })
                                .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                    }
                });
        }

        public static void ShowMagicDamageText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_MagicDamage.prefab";
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(path, self.InstanceId, true,
                (gameObject, instanceId) =>
                {
                    if (instanceId != self.InstanceId)
                    {
                        if (gameObject != null)
                        {
                            UnityEngine.Object.DestroyImmediate(gameObject);
                        }

                        return;
                    }

                    gameObject.transform.SetParent(self.Root().GetComponent<GlobalComponent>().BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().SetText(text);
                    }

                    gameObject.transform.position = head.position;

                    if (textTransform != null)
                    {
                        textTransform.localPosition = Vector3.zero;
                        Sequence seq = DOTween.Sequence();
                        seq.Append(textTransform.DOLocalMoveY(100f, 1.0f).SetEase(Ease.OutQuad))
                                .OnUpdate(() =>
                                {
                                    if (gameObject != null && head != null)
                                    {
                                        gameObject.transform.position = head.position;
                                    }
                                })
                                .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                    }
                });
        }

        public static void ShowCriDamageText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_CriDamage.prefab";
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(path, self.InstanceId, true,
                (gameObject, instanceId) =>
                {
                    if (instanceId != self.InstanceId)
                    {
                        if (gameObject != null)
                        {
                            UnityEngine.Object.DestroyImmediate(gameObject);
                        }

                        return;
                    }

                    gameObject.transform.SetParent(self.Root().GetComponent<GlobalComponent>().BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().SetText(text);
                    }

                    gameObject.transform.position = head.position;

                    if (textTransform != null)
                    {
                        textTransform.localPosition = Vector3.zero;
                        Sequence seq = DOTween.Sequence();
                        seq.Append(textTransform.DOLocalMoveY(100f, 1.0f).SetEase(Ease.OutQuad))
                                .OnUpdate(() =>
                                {
                                    if (gameObject != null && head != null)
                                    {
                                        gameObject.transform.position = head.position;
                                    }
                                })
                                .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                    }
                });
        }

        public static void ShowRecoverText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_Recover.prefab";
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(path, self.InstanceId, true,
                (gameObject, instanceId) =>
                {
                    if (instanceId != self.InstanceId)
                    {
                        if (gameObject != null)
                        {
                            UnityEngine.Object.DestroyImmediate(gameObject);
                        }

                        return;
                    }

                    gameObject.transform.SetParent(self.Root().GetComponent<GlobalComponent>().BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().SetText(text);
                    }

                    gameObject.transform.position = head.position;

                    if (textTransform != null)
                    {
                        textTransform.localPosition = Vector3.zero;
                        Sequence seq = DOTween.Sequence();
                        seq.Append(textTransform.DOLocalMoveY(100f, 1.0f).SetEase(Ease.OutQuad))
                                .OnUpdate(() =>
                                {
                                    if (gameObject != null && head != null)
                                    {
                                        gameObject.transform.position = head.position;
                                    }
                                })
                                .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                    }
                });
        }

        public static void ShowNormalText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_Normal.prefab";
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(path, self.InstanceId, true,
                (gameObject, instanceId) =>
                {
                    if (instanceId != self.InstanceId)
                    {
                        if (gameObject != null)
                        {
                            UnityEngine.Object.DestroyImmediate(gameObject);
                        }

                        return;
                    }

                    gameObject.transform.SetParent(self.Root().GetComponent<GlobalComponent>().BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().SetText(text);
                    }

                    gameObject.transform.position = head.position;

                    if (textTransform != null)
                    {
                        textTransform.localPosition = Vector3.zero;
                        Sequence seq = DOTween.Sequence();
                        seq.Append(textTransform.DOLocalMoveY(100f, 1.0f).SetEase(Ease.OutQuad))
                                .OnUpdate(() =>
                                {
                                    if (gameObject != null && head != null)
                                    {
                                        gameObject.transform.position = head.position;
                                    }
                                })
                                .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                    }
                });
        }

        public static void ShowTipText(this FloatingTextComponent self, string text)
        {
            string path = "Assets/Bundles/UI/Blood/Text_Tip.prefab";
            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(path, self.InstanceId, true,
                (gameObject, instanceId) =>
                {
                    if (instanceId != self.InstanceId)
                    {
                        if (gameObject != null)
                        {
                            UnityEngine.Object.DestroyImmediate(gameObject);
                        }

                        return;
                    }

                    gameObject.transform.SetParent(self.Root().GetComponent<GlobalComponent>().PopUpRoot);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.GetComponent<TMP_Text>().SetText(text);
                    gameObject.transform.localPosition = Vector3.zero;

                    Sequence seq = DOTween.Sequence();
                    // 增强向上飘动效果，增加移动距离和时间
                    seq.Append(gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + 200f, 2.0f).SetEase(Ease.OutQuad))
                            // .Join(gameObject.GetComponent<TMP_Text>().DOFade(0, 1.0f))
                            .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                });
        }
    }
}