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
            Vector2 startPos = Vector2.zero;

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

                startPos = uiMonsterHpComponent.GameObject.GetComponent<RectTransform>().localPosition;
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

                startPos = uiHeroHpComponent.GameObject.GetComponent<RectTransform>().localPosition;
            }

            if (args.DamageType == DamageType.Normal)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowDamageText((args.OldValue - args.NewValue).ToString(), startPos);
            }

            if (args.DamageType == DamageType.Critical)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowCriDamageText((args.OldValue - args.NewValue).ToString(), startPos);
            }

            if (args.DamageType == DamageType.Recover)
            {
                unit.Root().GetComponent<FloatingTextComponent>().ShowRecoverText((args.NewValue - args.OldValue).ToString(), startPos);
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

        public static void ShowDamageText(this FloatingTextComponent self, string text, Vector2 startPos)
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
                    gameObject.GetComponent<TMP_Text>().text = text;
                    gameObject.transform.localPosition = startPos;

                    Sequence seq = DOTween.Sequence();
                    seq.Append(gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + 100f, 1.0f).SetEase(Ease.OutQuad))
                            // .Join(gameObject.GetComponent<TMP_Text>().DOFade(0, 1.0f))
                            .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                });
        }

        public static void ShowCriDamageText(this FloatingTextComponent self, string text, Vector2 startPos)
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
                    gameObject.GetComponent<TMP_Text>().text = text;
                    gameObject.transform.localPosition = startPos;

                    Sequence seq = DOTween.Sequence();
                    seq.Append(gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + 100f, 1.0f).SetEase(Ease.OutQuad))
                            // .Join(gameObject.GetComponent<TMP_Text>().DOFade(0, 1.0f))
                            .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                });
        }

        public static void ShowRecoverText(this FloatingTextComponent self, string text, Vector2 startPos)
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
                    gameObject.GetComponent<TMP_Text>().text = text;
                    gameObject.transform.localPosition = startPos;

                    Sequence seq = DOTween.Sequence();
                    seq.Append(gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + 100f, 1.0f).SetEase(Ease.OutQuad))
                            // .Join(gameObject.GetComponent<TMP_Text>().DOFade(0, 1.0f))
                            .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
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
                    seq.Append(gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + 200f, 1.0f).SetEase(Ease.OutQuad))
                            // .Join(gameObject.GetComponent<TMP_Text>().DOFade(0, 1.0f))
                            .OnComplete(() => { self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject); });
                });
        }
    }
}