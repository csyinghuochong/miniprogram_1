using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawRewardPreviewComponent))]
    public static partial class UILotteryDrawRewardPreviewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawRewardPreviewComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            
             // ConfigData.LotteryDrawDropId
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawRewardPreviewComponent self)
        {
        }
    }
}