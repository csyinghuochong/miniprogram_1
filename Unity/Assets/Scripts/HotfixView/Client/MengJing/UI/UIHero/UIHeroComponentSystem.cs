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

            self.Transform_PanelRoot = rc.Get<GameObject>("Transform_PanelRoot").transform;
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();

            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_HeroList = rc.Get<GameObject>("Button_HeroList").GetComponent<Button>();
            self.Button_Formation = rc.Get<GameObject>("Button_Formation").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHero); });
            self.Button_Hero.AddListener(() => { self.ShowPanel(1); });
            self.Button_HeroList.AddListener(() => { self.ShowPanel(2); });
            self.Button_Formation.AddListener(() => { self.ShowPanel(3); });

            self.UpdateGold();
            self.UpdateDiamond();
            self.ShowPanel(1);
        }

        [EntitySystem]
        private static void Destroy(this UIHeroComponent self)
        {
        }

        private static void ShowPanel(this UIHeroComponent self, int panel)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            
            self.Button_Hero.transform.Find("Image_On").gameObject.SetActive(panel == 1);
            self.Button_Hero.transform.Find("Image_Off").gameObject.SetActive(panel != 1);
            self.Button_HeroList.transform.Find("Image_On").gameObject.SetActive(panel == 2);
            self.Button_HeroList.transform.Find("Image_Off").gameObject.SetActive(panel != 2);
            self.Button_Formation.transform.Find("Image_On").gameObject.SetActive(panel == 3);
            self.Button_Formation.transform.Find("Image_Off").gameObject.SetActive(panel != 3);

            UICommonHelper.HideChild(self.Transform_PanelRoot.gameObject);
            if (panel == 1)
            {
                if (self.UIHeroInfoComponent == null)
                {
                    GameObject go = UnityEngine.Object.Instantiate(rc.Get<GameObject>("UIHeroInfo"), self.Transform_PanelRoot);
                    self.UIHeroInfoComponent = self.AddComponent<UIHeroInfoComponent, GameObject>(go);
                }

                self.UIHeroInfoComponent.UpdateHeroList();
                self.UIHeroInfoComponent.SelectFirstHero();
                self.UIHeroInfoComponent.GameObject.SetActive(true);
            }

            if (panel == 2)
            {
                if (self.UIHeroListComponent == null)
                {
                    GameObject go = UnityEngine.Object.Instantiate(rc.Get<GameObject>("UIHeroList"), self.Transform_PanelRoot);
                    self.UIHeroListComponent = self.AddComponent<UIHeroListComponent, GameObject>(go);
                }

                self.UIHeroListComponent.SetShowType(1);
                self.UIHeroListComponent.UpdateHaveHeroCount();
                self.UIHeroListComponent.GameObject.SetActive(true);
            }

            if (panel == 3)
            {
                if (self.UIHeroFormationComponent == null)
                {
                    GameObject go = UnityEngine.Object.Instantiate(rc.Get<GameObject>("UIHeroFormation"), self.Transform_PanelRoot);
                    self.UIHeroFormationComponent = self.AddComponent<UIHeroFormationComponent, GameObject>(go);
                }

                self.UIHeroFormationComponent.SetShowType(1);
                self.UIHeroFormationComponent.UpdateSlotItemList();
                self.UIHeroFormationComponent.GameObject.SetActive(true);
            }
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