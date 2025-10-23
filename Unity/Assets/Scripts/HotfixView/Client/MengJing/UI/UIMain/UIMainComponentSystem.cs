using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class DataUpdate_UpdateUserData_UIMainRefresh : AEvent<Scene, UpdateUserData>
    {
        protected override async ETTask Run(Scene scene, UpdateUserData args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();

            if (args.UserDataType == UserDataType.Lv)
            {
                uiMainComponent.UpdatePlayerLv();
            }

            if (args.UserDataType == UserDataType.Exp)
            {
                uiMainComponent.UpdateExp();
            }

            if (args.UserDataType == UserDataType.Gold)
            {
                uiMainComponent.UpdateGold();
            }

            if (args.UserDataType == UserDataType.Diamond)
            {
                uiMainComponent.UpdateDiamond();
            }

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIMainComponent))]
    [FriendOf(typeof(UIMainComponent))]
    public static partial class UIMainComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerLv = rc.Get<GameObject>("Text_PlayerLv").GetComponent<TMP_Text>();
            self.Text_FPS = rc.Get<GameObject>("Text_FPS").GetComponent<TMP_Text>();
            self.Text_Gold = rc.Get<GameObject>("Text_Gold").GetComponent<TMP_Text>();
            self.Text_Diamond = rc.Get<GameObject>("Text_Diamond").GetComponent<TMP_Text>();
            self.Button_Speed = rc.Get<GameObject>("Button_Speed").GetComponent<Button>();
            self.Button_GM = rc.Get<GameObject>("Button_GM").GetComponent<Button>();
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Bag = rc.Get<GameObject>("Button_Bag").GetComponent<Button>();
            self.Slider_Exp = rc.Get<GameObject>("Slider_Exp").GetComponent<Slider>();
            self.Text_Exp = rc.Get<GameObject>("Text_Exp").GetComponent<TMP_Text>();

            self.Button_Speed.onClick.AddListener(() => { self.OnButton_Speed(); });
            self.Button_GM.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIGM).Coroutine(); });
            self.Button_Hero.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHero).Coroutine(); });
            self.Button_Bag.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIBag).Coroutine(); });

            self.UpdatePlayerName();
            self.UpdatePlayerLv();
            self.UpdateGold();
            self.UpdateDiamond();
            self.UpdateExp();
            Application.targetFrameRate = 60;
        }

        [EntitySystem]
        private static void Update(this UIMainComponent self)
        {
            self.UpdateFPS();
        }

        private static void UpdateFPS(this UIMainComponent self)
        {
            self.TimeLeft -= Time.deltaTime;
            self.Accumulator += Time.timeScale / Time.deltaTime;
            self.FrameCount++;
            if (self.TimeLeft <= 0f)
            {
                self.FPS = self.Accumulator / self.FrameCount;
                self.Text_FPS.SetTextFormat("FPS:{0}", (int)self.FPS);

                self.TimeLeft = self.UpdateInterval;
                self.Accumulator = 0f;
                self.FrameCount = 0;
            }
        }

        private static void OnButton_Speed(this UIMainComponent self)
        {
            self.SpeedLevel++;
            if (self.SpeedLevel > 3)
            {
                self.SpeedLevel = 0;
            }

            switch (self.SpeedLevel)
            {
                case 0:
                    Time.timeScale = 0f;
                    break;
                case 1:
                    Time.timeScale = 1f;
                    break;
                case 2:
                    Time.timeScale = 2f;
                    break;
                case 3:
                    Time.timeScale = 3f;
                    break;
            }

            self.Button_Speed.GetComponentInChildren<TMP_Text>().SetTextFormat("x{0}", self.SpeedLevel);
        }

        public static void UpdatePlayerName(this UIMainComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            self.Text_PlayerName.SetText(userInfoComponent.PlayerName);
        }

        public static void UpdatePlayerLv(this UIMainComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            self.Text_PlayerLv.SetText(userInfoComponent.Lv);

            self.UpdateExp();
        }

        public static void UpdateGold(this UIMainComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            if (userInfoComponent.Gold < 1000)
            {
                self.Text_Gold.SetText(userInfoComponent.Gold);
            }
            else
            {
                self.Text_Gold.SetTextFormat("{0}K", userInfoComponent.Gold / 1000);
            }
        }

        public static void UpdateDiamond(this UIMainComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            if (userInfoComponent.Diamond < 1000)
            {
                self.Text_Diamond.SetText(userInfoComponent.Diamond);
            }
            else
            {
                self.Text_Diamond.SetTextFormat("{0}K", userInfoComponent.Diamond / 1000);
            }
        }

        public static void UpdateExp(this UIMainComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            int max = ExpConfigCategory.Instance.Get(userInfoComponent.Lv).PlayerUpExp;
            self.Slider_Exp.value = userInfoComponent.Exp * 1f / max;
            self.Text_Exp.SetTextFormat("{0}/{1}", userInfoComponent.Exp, max);
        }
    }
}