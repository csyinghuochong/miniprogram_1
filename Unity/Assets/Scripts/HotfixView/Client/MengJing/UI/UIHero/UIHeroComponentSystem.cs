using System.Collections.Generic;
using Cysharp.Text;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class DataUpdate_UpdateUserData_UpdateUIHero : AEvent<Scene, UpdateUserData>
    {
        protected override async ETTask Run(Scene scene, UpdateUserData args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();

            if (args.UserDataType == UserDataType.Gold)
            {
                uiHeroComponent.UpdateGold();
            }

            if (args.UserDataType == UserDataType.Diamond)
            {
                uiHeroComponent.UpdateDiamond();
            }

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class HeroFormationUpdate_UpdateUIHero : AEvent<Scene, HeroFormationUpdate>
    {
        protected override async ETTask Run(Scene root, HeroFormationUpdate args)
        {
            UI ui = root.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();
            uiHeroComponent.UIHeroInfoComponent?.UpdateHeroList();

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class HeroUpdate_UpdateUIHero : AEvent<Scene, HeroUpdate>
    {
        protected override async ETTask Run(Scene root, HeroUpdate args)
        {
            UI ui = root.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();
            uiHeroComponent.UIHeroInfoComponent?.UpdateHeroInfo().Coroutine();

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class InventoryUpdate_UIHeroRefresh : AEvent<Scene, InventoryUpdate>
    {
        protected override async ETTask Run(Scene scene, InventoryUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();
            uiHeroComponent.UIHeroInfoComponent?.UpdateItemList();

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIHeroComponent))]
    [FriendOf(typeof(UIHeroComponent))]
    public static partial class UIHeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Transform_PanelRoot = rc.Get<GameObject>("Transform_PanelRoot").transform;
            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();

            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_HeroList = rc.Get<GameObject>("Button_HeroList").GetComponent<Button>();
            self.Button_Formation = rc.Get<GameObject>("Button_Formation").GetComponent<Button>();

            self.UIHeroInfoComponent = self.AddComponent<UIHeroInfoComponent, GameObject>(UnityEngine.Object.Instantiate(rc.Get<GameObject>("UIHeroInfo"), self.Transform_PanelRoot));
            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHero); });
            self.Button_Hero.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHero).Coroutine(); });
            self.Button_HeroList.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroList).Coroutine(); });
            self.Button_Formation.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFormation).Coroutine(); });

            self.UpdateGold();
            self.UpdateDiamond();
        }

        [EntitySystem]
        private static void Destroy(this UIHeroComponent self)
        {
        }

        public static void UpdateGold(this UIHeroComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            self.Text_Type_Gold.SetText(userInfoComponent.Gold);
        }

        public static void UpdateDiamond(this UIHeroComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            self.Text_Type_Diamond.SetText(userInfoComponent.Diamond);
        }
    }
}