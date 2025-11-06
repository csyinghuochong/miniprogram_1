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

    [Event(SceneType.Demo)]
    public class UpdateTimeScale_UIMainRefresh : AEvent<Scene, UpdateTimeScale>
    {
        protected override async ETTask Run(Scene scene, UpdateTimeScale args)
        {
            Time.timeScale = args.TimeScale;

            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();
            uiMainComponent.UpdateTimeScale();

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

            self.UIJoystick = rc.Get<GameObject>("UIJoystick");
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerLv = rc.Get<GameObject>("Text_PlayerLv").GetComponent<TMP_Text>();
            self.Text_FPS = rc.Get<GameObject>("Text_FPS").GetComponent<TMP_Text>();
            self.Text_Gold = rc.Get<GameObject>("Text_Gold").GetComponent<TMP_Text>();
            self.Text_Diamond = rc.Get<GameObject>("Text_Diamond").GetComponent<TMP_Text>();
            self.Button_Recall = rc.Get<GameObject>("Button_Recall").GetComponent<Button>();
            self.Button_StartLevel = rc.Get<GameObject>("Button_StartLevel").GetComponent<Button>();
            self.Button_Speed = rc.Get<GameObject>("Button_Speed").GetComponent<Button>();
            self.Button_GM = rc.Get<GameObject>("Button_GM").GetComponent<Button>();
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Bag = rc.Get<GameObject>("Button_Bag").GetComponent<Button>();
            self.UIMainSkill = rc.Get<GameObject>("UIMainSkill");
            self.Button_Boss = rc.Get<GameObject>("Button_Boss").GetComponent<Button>();
            self.Slider_Exp = rc.Get<GameObject>("Slider_Exp").GetComponent<Slider>();
            self.Text_Exp = rc.Get<GameObject>("Text_Exp").GetComponent<TMP_Text>();

            self.UIJoystickComponent = self.AddComponent<UIJoystickComponent, GameObject>(self.UIJoystick);
            self.Button_Recall.AddListener(() => { EnterMapHelper.RequestTransfer(self.Root(), MapTypeEnum.MainCityScene).Coroutine(); });
            self.Button_StartLevel.AddListener(() => { EnterMapHelper.RequestTransfer(self.Root(), MapTypeEnum.LocalLevel).Coroutine(); });
            self.Button_Speed.AddListener(() => { self.OnButton_Speed(); });
            self.Button_GM.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIGM).Coroutine(); });
            self.Button_Hero.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHero).Coroutine(); });
            self.Button_Bag.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIBag).Coroutine(); });
            self.Button_Boss.AddListener(() => { ClientLevelHelper.EnterBossRoom(self.Root()).Coroutine(); });

            Application.targetFrameRate = 60;
        }

        [EntitySystem]
        private static void Update(this UIMainComponent self)
        {
            self.UpdateFPS();
        }

        // 加载场景之前
        public static void BeforeEnterScene(this UIMainComponent self, int lastSceneType)
        {
        }

        // 场景和角色都加载完成后
        public static void AfterEnterScene(this UIMainComponent self, int sceneType)
        {
            self.Button_StartLevel.gameObject.SetActive(sceneType == MapTypeEnum.MainCityScene);
            self.Button_Recall.gameObject.SetActive(sceneType == MapTypeEnum.LocalLevel);
            self.UIMainSkill.SetActive(sceneType == MapTypeEnum.LocalLevel);
            
            self.UpdatePlayerName();
            self.UpdatePlayerLv();
            self.UpdateGold();
            self.UpdateDiamond();
            self.UpdateExp();
            
            self.UIJoystickComponent.AfterEnterScene(sceneType);
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
            float timeScale = Time.timeScale;

            if (timeScale <= 0.5f)
            {
                timeScale = 1f;
            }
            else if (timeScale <= 1f)
            {
                timeScale = 2f;
            }
            else if (timeScale <= 2f)
            {
                timeScale = 3f;
            }
            else
            {
                timeScale = 0;
            }

            C2M_SetTimeScale request = C2M_SetTimeScale.Create();
            request.TimeScale = timeScale;
            self.Root().GetComponent<ClientSenderComponent>().Call(request).Coroutine();
        }

        public static void UpdateTimeScale(this UIMainComponent self)
        {
            self.Button_Speed.GetComponentInChildren<TMP_Text>().SetTextFormat("x{0:0.#}", Time.timeScale);
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