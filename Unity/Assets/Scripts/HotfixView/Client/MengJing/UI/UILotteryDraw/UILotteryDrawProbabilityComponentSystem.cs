using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawProbabilityComponent))]
    public static partial class UILotteryDrawProbabilityComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawProbabilityComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawProbabilityComponent self)
        {
        }
    }
}