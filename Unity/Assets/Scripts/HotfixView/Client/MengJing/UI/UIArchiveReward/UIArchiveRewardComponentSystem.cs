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



        }


    }
}