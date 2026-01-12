using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawWishComponent))]
    [FriendOf(typeof(UILotteryDrawWishComponent))]
    public static partial class UILotteryDrawWishComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawWishComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.GameObject.SetActive(false); });

            //ConfigData.LotteryDrawWishItemIdList
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawWishComponent self)
        {
        }
    }
}