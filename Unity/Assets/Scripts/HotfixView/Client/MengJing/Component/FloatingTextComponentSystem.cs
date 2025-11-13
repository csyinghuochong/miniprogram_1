using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ET.Client
{
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

            if (args.DamageType == DamageType.Normal)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowDamageText((args.OldValue - args.NewValue).ToString(), head);
            }

            if (args.DamageType == DamageType.Critical)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowCriDamageText((args.OldValue - args.NewValue).ToString(), head);
            }

            if (args.DamageType == DamageType.Recover)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowRecoverText((args.NewValue - args.OldValue).ToString(), head);
            }
        }
    }

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

        public static void ShowDamageText(this FloatingTextComponent self, string text, Transform head)
        {
            string path = "Assets/Bundles/UI/Blood/Text_Damage.prefab";
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

                    gameObject.transform.SetParent(GlobalComponent.Instance.BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().text = text;
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

                    gameObject.transform.SetParent(GlobalComponent.Instance.BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().text = text;
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

                    gameObject.transform.SetParent(GlobalComponent.Instance.BloodText_Layer0.transform);
                    gameObject.transform.localScale = Vector3.one;

                    Transform textTransform = gameObject.transform.Find("Text");
                    if (textTransform != null)
                    {
                        textTransform.GetComponent<TMP_Text>().text = text;
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

                    gameObject.transform.SetParent(GlobalComponent.Instance.PopUpRoot);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.GetComponent<TMP_Text>().text = text;
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