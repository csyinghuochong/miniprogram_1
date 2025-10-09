using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UITeamComponent))]
    [FriendOf(typeof(UITeamComponent))]
    public static partial class UITeamComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UITeamComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Formation = rc.Get<GameObject>("Button_Formation").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UITeam); });
            self.Button_Hero.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHero).Coroutine(); });
            self.Button_Formation.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFormation).Coroutine(); });
        }
    }
}