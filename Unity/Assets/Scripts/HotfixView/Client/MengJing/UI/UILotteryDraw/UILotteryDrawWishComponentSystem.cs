using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawWishComponent))]
    public static partial class UILotteryDrawWishComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawWishComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            
             // ConfigData.LotteryDrawWishItemIdList
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawWishComponent self)
        {
        }
    }
}