using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawProbabilityComponent))]
    [FriendOf(typeof(UILotteryDrawProbabilityComponent))]
    public static partial class UILotteryDrawProbabilityComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawProbabilityComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.GameObject.SetActive(false); });

            // 直接写上去
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawProbabilityComponent self)
        {
        }
    }
}