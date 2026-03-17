using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class TaskCommit_ShowReward : AEvent<Scene, TaskCommit>
    {
        protected override async ETTask Run(Scene scene, TaskCommit args)
        {
            TaskConfig config = TaskConfigCategory.Instance.Get(args.TaskConfigId);

            UI ui = await scene.GetComponent<UIComponent>().Create(UIType.UIGetReward);
            UIGetRewardComponent uiGetRewardComponent = ui.GetComponent<UIGetRewardComponent>();
            uiGetRewardComponent.OnInit(config.RewardItem);

            await ETTask.CompletedTask;
        }
    }

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

        public static void OnInit(this UIGetRewardComponent self, List<RewardItem> rewardItems)
        {
            for (int i = 0; i < rewardItems.Count; i++)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                newItem.UpdateInfo(rewardItems[i].ItemId, rewardItems[i].ItemNum).Coroutine();
            }
        }
        
        public static void OnInit(this UIGetRewardComponent self, RewardItem[] rewardItems)
        {
            for (int i = 0; i < rewardItems.Length; i++)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                newItem.UpdateInfo(rewardItems[i].ItemId, rewardItems[i].ItemNum).Coroutine();
            }
        }
    }
}