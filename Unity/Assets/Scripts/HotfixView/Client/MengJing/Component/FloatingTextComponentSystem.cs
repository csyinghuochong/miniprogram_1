using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FloatingTextComponent))]
    [FriendOf(typeof(FloatingTextComponent))]
    public static partial class FloatingTextComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FloatingTextComponent self)
        {
        }

        [EntitySystem]
        private static void FixedUpdate(this FloatingTextComponent self)
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
                            .OnComplete(() =>
                            {
                                self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(path, gameObject);
                            });
                });
        }
    }
}