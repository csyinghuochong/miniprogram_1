using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemRewardTipComponent))]
    [FriendOf(typeof(UIItemRewardTipComponent))]
    public static partial class UIItemRewardTipComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemRewardTipComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").GetComponent<Transform>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
        }

        public static void OnInit(this UIItemRewardTipComponent self, Vector3 vector3, RewardItem[] rewardItems)
        {
            self.Content_UICommonItem.localPosition = vector3;
            for (int i = 0; i < rewardItems.Length; i++)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                newItem.UpdateInfo(rewardItems[i].ItemId, rewardItems[i].ItemNum).Coroutine();
            }
        }
    }
}