using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFormationComponent))]
    [FriendOf(typeof(UIFormationComponent))]
    public static partial class UIFormationComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIFormationComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIFormation); });
        }
    }
}