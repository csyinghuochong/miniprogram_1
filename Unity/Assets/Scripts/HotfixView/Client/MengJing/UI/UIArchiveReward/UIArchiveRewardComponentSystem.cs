using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIArchiveRewardComponent))]
    [FriendOf(typeof(UIArchiveRewardComponent))]
    public static partial class UIArchiveRewardComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIArchiveRewardComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIArchiveRewardItem = rc.Get<GameObject>("Content_UIArchiveRewardItem").transform;
            self.UIArchiveRewardItem = rc.Get<GameObject>("UIArchiveRewardItem");
            self.Text_CurrentPoints = rc.Get<GameObject>("Text_CurrentPoints").GetComponent<TMP_Text>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIArchiveReward); });
        }

        [EntitySystem]
        private static void Destroy(this UIArchiveRewardComponent self)
        {
            self.UIArchiveRewardItemList.Clear();
            self.UIArchiveRewardItem = null;
        }
        
        

    }
}