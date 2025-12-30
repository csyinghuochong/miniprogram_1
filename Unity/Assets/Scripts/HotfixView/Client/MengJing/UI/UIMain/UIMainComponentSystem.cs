using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    # region 事件监听

    [Event(SceneType.Demo)]
    public class ChatUpdate_UIMainRefresh : AEvent<Scene, ChatUpdate>
    {
        protected override async ETTask Run(Scene scene, ChatUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();
            uiMainComponent.UpdateUIMainChatItemList();

            await ETTask.CompletedTask;
        }
    }

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

    [Event(SceneType.Demo)]
    public class TaskUpdate_UIMainRefresh : AEvent<Scene, TaskUpdate>
    {
        protected override async ETTask Run(Scene scene, TaskUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();
            uiMainComponent.UpdateMainTask();

            await ETTask.CompletedTask;
        }
    }

    [NumericWatcher(SceneType.Current, NumericType.CurrentLevelId)]
    public class NumericWatcher_CurrentLevelId_UIMainRefresh : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            if (!LevelConfigCategory.Instance.DataMap.ContainsKey((int)args.NewValue))
            {
                return;
            }

            LevelConfig levelConfig = LevelConfigCategory.Instance.Get((int)args.NewValue);

            // unit.Root().GetComponent<FloatingTextComponent>().ShowTipText(levelConfig.LevelName);

            UI ui = unit.Root().GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();
            uiMainComponent.UpdateLevelProgress();
            uiMainComponent.UIMiniMapComponent?.UpdateLevelName();
        }
    }

    [NumericWatcher(SceneType.Current, NumericType.CurrentWaveIndex)]
    public class NumericWatcher_CurrentWaveIndex_UIMainRefresh : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            // unit.Root().GetComponent<FloatingTextComponent>().ShowTipText(ZString.Format("第{0}波", args.NewValue));

            UI ui = unit.Root().GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();
            uiMainComponent.UpdateLevelProgress();
        }
    }

    [NumericWatcher(SceneType.Current, NumericType.CurrentWaveKillMonsterNum)]
    public class NumericWatcher_CurrentWaveKillMonsterNum_UIMainRefresh : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            UI ui = unit.Root().GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();
            uiMainComponent.UpdateLevelProgress();
        }
    }
    
    [NumericWatcher(SceneType.Current, NumericType.CombatPower)]
    public class NumericWatcher_CombatPower_UIMainRefresh : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            UI ui = unit.Root().GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui == null)
            {
                return;
            }

            UIMainComponent uiMainComponent = ui.GetComponent<UIMainComponent>();
            uiMainComponent.UpdateCombatPower();
        }
    }

    #endregion

    [EntitySystemOf(typeof(UIMainComponent))]
    [FriendOf(typeof(UIMainComponent))]
    public static partial class UIMainComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_UID = rc.Get<GameObject>("Text_UID").GetComponent<TMP_Text>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_CP = rc.Get<GameObject>("Text_CP").GetComponent<TMP_Text>();
            self.Text_PlayerLv = rc.Get<GameObject>("Text_PlayerLv").GetComponent<TMP_Text>();
            self.Text_FPS = rc.Get<GameObject>("Text_FPS").GetComponent<TMP_Text>();
            self.Text_Ping = rc.Get<GameObject>("Text_Ping").GetComponent<TMP_Text>();
            self.Text_Gold = rc.Get<GameObject>("Text_Gold").GetComponent<TMP_Text>();
            self.Text_Diamond = rc.Get<GameObject>("Text_Diamond").GetComponent<TMP_Text>();
            self.Image_TaskCompleted = rc.Get<GameObject>("Image_TaskCompleted").GetComponent<Image>();
            self.Text_TaskName = rc.Get<GameObject>("Text_TaskName").GetComponent<TMP_Text>();
            self.Text_TaskProgress = rc.Get<GameObject>("Text_TaskProgress").GetComponent<TMP_Text>();
            self.Button_TaskCommit = rc.Get<GameObject>("Button_TaskCommit").GetComponent<Button>();
            self.EventTrigger_TaskReward = rc.Get<GameObject>("EventTrigger_TaskReward").GetComponent<EventTrigger>();
            self.Button_Recall = rc.Get<GameObject>("Button_Recall").GetComponent<Button>();
            self.Button_PickUpDropItem = rc.Get<GameObject>("Button_PickUpDropItem").GetComponent<Button>();
            self.Button_StartLevel = rc.Get<GameObject>("Button_StartLevel").GetComponent<Button>();
            self.Button_Speed = rc.Get<GameObject>("Button_Speed").GetComponent<Button>();
            self.Button_GM = rc.Get<GameObject>("Button_GM").GetComponent<Button>();
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Bag = rc.Get<GameObject>("Button_Bag").GetComponent<Button>();
            self.UILevelProgress = rc.Get<GameObject>("UILevelProgress");
            self.Button_Boss = rc.Get<GameObject>("Button_Boss").GetComponent<Button>();
            self.Slider_Exp = rc.Get<GameObject>("Slider_Exp").GetComponent<Slider>();
            self.Text_Exp = rc.Get<GameObject>("Text_Exp").GetComponent<TMP_Text>();
            self.Content_UIMainChatItem = rc.Get<GameObject>("Content_UIMainChatItem").transform;
            self.UIMainChatItem = rc.Get<GameObject>("UIMainChatItem");
            self.UIMainChatItem.SetActive(false);
            self.Button_OnChat = rc.Get<GameObject>("Button_OnChat").GetComponent<Button>();
            self.Button_Friend = rc.Get<GameObject>("Button_Friend").GetComponent<Button>();

            self.UIMiniMapComponent = self.AddComponent<UIMiniMapComponent, GameObject>(rc.Get<GameObject>("UIMiniMap"));
            self.UIJoystickComponent = self.AddComponent<UIJoystickComponent, GameObject>(rc.Get<GameObject>("UIJoystick"));
            self.UIMainSkillComponent = self.AddComponent<UIMainSkillComponent, GameObject>(rc.Get<GameObject>("UIMainSkill"));
            self.Button_TaskCommit.AddListener(() => { self.OnButton_TaskCommit(); });
            self.EventTrigger_TaskReward.AddEventTrigger((p) => { self.OnTaskRewardPointerDown(p).Coroutine(); }, EventTriggerType.PointerDown);
            self.EventTrigger_TaskReward.AddEventTrigger(self.OnTaskRewardPointerUp, EventTriggerType.PointerUp);
            self.Button_Recall.AddListener(() => { EnterMapHelper.RequestTransfer(self.Root(), MapType.MainCity).Coroutine(); });
            self.Button_PickUpDropItem.AddListener(() => { self.OnButton_PickUpDropItem(); });
            self.Button_StartLevel.AddListener(() => { EnterMapHelper.RequestTransfer(self.Root(), MapType.LocalLevel).Coroutine(); });
            self.Button_Speed.AddListener(() => { self.OnButton_Speed(); });
            self.Button_GM.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIGM).Coroutine(); });
            self.Button_Hero.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHero).Coroutine(); });
            self.Button_Bag.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIBag).Coroutine(); });
            self.Button_Boss.AddListener(() => { self.OnBoss().Coroutine(); });
            self.Button_OnChat.AddListener(() => { self.OnButton_OnChat().Coroutine(); });
            self.Button_Friend.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFriend).Coroutine(); });
        }

        [EntitySystem]
        private static void Update(this UIMainComponent self)
        {
            self.UpdateFPS();

            self.Text_Ping.SetTextFormat("{0}ms", TimeInfo.Instance.Ping);
        }

        // 加载场景之前
        public static void BeforeEnterScene(this UIMainComponent self, MapType mapType)
        {
            self.UIMainSkillComponent.BeforeEnterScene(mapType);
        }

        // 场景和角色都加载完成后
        public static void AfterEnterScene(this UIMainComponent self, MapType mapType)
        {
            self.Button_StartLevel.gameObject.SetActive(mapType == MapType.MainCity);
            self.Button_Recall.gameObject.SetActive(mapType == MapType.LocalLevel);
            self.UILevelProgress.gameObject.SetActive(mapType == MapType.LocalLevel);

            self.UpdatePlayerName();
            self.UpdateCombatPower();
            self.UpdatePlayerLv();
            self.UpdateGold();
            self.UpdateDiamond();
            self.UpdateExp();
            self.UpdateMainTask();
            self.UpdateUIMainChatItemList();

            self.UIMiniMapComponent.AfterEnterScene(mapType);
            self.UIJoystickComponent.AfterEnterScene(mapType);
            self.UIMainSkillComponent.AfterEnterScene(mapType);
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

            if (timeScale < 1.5f)
            {
                timeScale = 2f;
            }
            else
            {
                timeScale = 1f;
            }

            C2M_SetTimeScale request = C2M_SetTimeScale.Create();
            request.TimeScale = timeScale;
            self.Root().GetComponent<ClientSenderComponent>().Call(request).Coroutine();
        }

        public static void UpdateTimeScale(this UIMainComponent self)
        {
            if (Time.timeScale < 1.5f)
            {
                self.Button_Speed.transform.Find("Image_1").gameObject.SetActive(true);
                self.Button_Speed.transform.Find("Image_2").gameObject.SetActive(false);
            }
            else
            {
                self.Button_Speed.transform.Find("Image_1").gameObject.SetActive(false);
                self.Button_Speed.transform.Find("Image_2").gameObject.SetActive(true);
            }
        }

        public static void UpdatePlayerName(this UIMainComponent self)
        {
            PlayerInfoComponent playerInfoComponent = self.Root().GetComponent<PlayerInfoComponent>();
            self.Text_UID.SetTextFormat("ID:{0}", playerInfoComponent.CurrentRoleId);

            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            self.Text_PlayerName.SetText(userInfoComponent.PlayerName);
        }

        public static void UpdateCombatPower(this UIMainComponent self)
        {
            NumericComponentC numericComponent = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<NumericComponentC>();
            self.Text_CP.SetText(numericComponent.GetAsLong(NumericType.CombatPower));
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

        public static void UpdateLevelProgress(this UIMainComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromClientScene(self.Root());
            NumericComponentC numericComponent = unit.GetComponent<NumericComponentC>();

            int currentLevelId = numericComponent.GetAsInt(NumericType.CurrentLevelId);
            int currentWaveIndex = numericComponent.GetAsInt(NumericType.CurrentWaveIndex);
            int currentWaveKillMonsterNum = numericComponent.GetAsInt(NumericType.CurrentWaveKillMonsterNum);
            if (!LevelConfigCategory.Instance.DataMap.ContainsKey(currentLevelId))
            {
                return;
            }

            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(currentLevelId);

            if (currentWaveIndex < 1 || currentWaveIndex > levelConfig.WaveIds.Length)
            {
                return;
            }

            WaveConfig waveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[currentWaveIndex - 1]);

            for (int i = 1; i < 6; i++)
            {
                Transform progress = self.UILevelProgress.transform.Find(ZString.Format("Image_LevelProgress_{0}", i));
                if (currentWaveIndex < i)
                {
                    progress.Find("Image_LevelProgress").GetComponent<Image>().fillAmount = 0;
                    progress.Find("Image_StartOff").gameObject.SetActive(true);
                    progress.Find("Image_StartOn").gameObject.SetActive(false);
                    progress.Find("Image_EndOff").gameObject.SetActive(true);
                    progress.Find("Image_EndOn").gameObject.SetActive(false);
                }
                else if (currentWaveIndex == i)
                {
                    progress.Find("Image_LevelProgress").GetComponent<Image>().fillAmount =
                            currentWaveKillMonsterNum * 1f / waveConfig.MonsterSpawnInfos.Length;
                    progress.Find("Image_StartOff").gameObject.SetActive(false);
                    progress.Find("Image_StartOn").gameObject.SetActive(true);
                    progress.Find("Image_EndOff").gameObject.SetActive(true);
                    progress.Find("Image_EndOn").gameObject.SetActive(false);
                }
                else
                {
                    progress.Find("Image_LevelProgress").GetComponent<Image>().fillAmount = 1;
                    progress.Find("Image_StartOff").gameObject.SetActive(false);
                    progress.Find("Image_StartOn").gameObject.SetActive(true);
                    progress.Find("Image_EndOff").gameObject.SetActive(false);
                    progress.Find("Image_EndOn").gameObject.SetActive(true);
                }
            }

            self.Button_Boss.gameObject.SetActive(false);
            if (currentWaveKillMonsterNum >= waveConfig.MonsterSpawnInfos.Length)
            {
                if (currentWaveIndex >= levelConfig.WaveIds.Length)
                {
                    // 击败最后一波怪物(包括Boss) 看看是继续下一关还是直接返回
                }
                else
                {
                    WaveConfig nextWaveConfig = WaveConfigCategory.Instance.Get(levelConfig.WaveIds[currentWaveIndex]);
                    if (nextWaveConfig.HaveBoss)
                    {
                        // 等待玩家进入Boss房间
                        self.Button_Boss.gameObject.SetActive(true);
                    }
                }
            }
        }

        public static async ETTask OnBoss(this UIMainComponent self)
        {
            int error = await ClientLevelHelper.EnterBossRoom(self.Root());
        }

        public static void UpdateMainTask(this UIMainComponent self)
        {
            TaskComponentC taskComponent = self.Root().GetComponent<TaskComponentC>();
            TaskPro taskPro = taskComponent.GetMainTask();
            if (taskPro == null)
            {
                return;
            }

            TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskPro.ConfigId);

            self.Text_TaskName.SetText(taskConfig.TaskName);
            self.Text_TaskProgress.SetTextFormat("{0}/{1}", taskPro.TaskTargetNum_1, taskConfig.TargetValue[0]); //先这样，后面应该根据不同的目标类型显示会有所调整
            self.Image_TaskCompleted.gameObject.SetActive(taskPro.TaskState == (int)TaskState.Completed);
            self.Button_TaskCommit.gameObject.SetActive(taskPro.TaskState == (int)TaskState.Completed);
        }

        private static void OnButton_TaskCommit(this UIMainComponent self)
        {
            TaskComponentC taskComponent = self.Root().GetComponent<TaskComponentC>();
            TaskPro taskPro = taskComponent.GetMainTask();
            if (taskPro == null)
            {
                return;
            }

            ClientTaskHelper.TaskCommit(self.Root(), taskPro.ConfigId).Coroutine();
        }

        private static async ETTask OnTaskRewardPointerDown(this UIMainComponent self, PointerEventData pdata)
        {
            TaskComponentC taskComponent = self.Root().GetComponent<TaskComponentC>();
            TaskPro taskPro = taskComponent.GetMainTask();
            if (taskPro == null)
            {
                return;
            }

            TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskPro.ConfigId);

            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemRewardTip);

            Vector2 localPoint;
            RectTransform canvas = self.GetParent<UI>().GameObject.GetComponent<RectTransform>();
            Camera uiCamera = self.Root().GetComponent<GlobalComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out localPoint);
            ui.GetComponent<UIItemRewardTipComponent>().OnInit(new Vector3(localPoint.x, localPoint.y, 0f), taskConfig.RewardItem);
        }

        private static void OnTaskRewardPointerUp(this UIMainComponent self, PointerEventData pdata)
        {
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemRewardTip);
        }

        public static void UpdateExp(this UIMainComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            int max = ExpConfigCategory.Instance.Get(userInfoComponent.Lv).PlayerUpExp;
            self.Slider_Exp.value = userInfoComponent.Exp * 1f / max;
            self.Text_Exp.SetTextFormat("{0}/{1}", userInfoComponent.Exp, max);
        }

        private static void OnButton_PickUpDropItem(this UIMainComponent self)
        {
            self.Root().GetComponent<PickUpDropItemComponent>().OnStarDrop();
        }

        public static void UpdateUIMainChatItemList(this UIMainComponent self)
        {
            ChatComponentC chatComponent = self.Root().GetComponent<ChatComponentC>();
            List<Chat> chatList = chatComponent.GetAllChatList();

            while (self.UIMainChatItemList.Count < chatList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIMainChatItem, self.Content_UIMainChatItem);
                UIMainChatItem newItem = self.AddChild<UIMainChatItem, GameObject>(go);
                self.UIMainChatItemList.Add(newItem);
            }

            for (int i = 0; i < chatList.Count; i++)
            {
                self.UIMainChatItemList[i].UpdateInfo(chatList[i]);
                self.UIMainChatItemList[i].GameObject.SetActive(true);
            }

            for (int i = chatList.Count; i < self.UIMainChatItemList.Count; i++)
            {
                self.UIMainChatItemList[i].GameObject.SetActive(false);
            }

            // 移动到底部
            Canvas.ForceUpdateCanvases();
            self.Content_UIMainChatItem.parent.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
            Canvas.ForceUpdateCanvases();
        }

        private static async ETTask OnButton_OnChat(this UIMainComponent self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIChat);
            ui.GetComponent<UIChatComponent>().SetShowType(0);
        }
    }
}