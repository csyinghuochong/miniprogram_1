using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIGetRewardComponent))]
    [FriendOf(typeof(UIGetRewardComponent))]
    public static partial class UIGetRewardComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIGetRewardComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIGetReward); });
        }
        
        [EntitySystem]
        private static void Destroy(this UIGetRewardComponent self)
        {
            self.UIRewardItemList.Clear();
            self.UICommonItem = null;
        }
    }
}