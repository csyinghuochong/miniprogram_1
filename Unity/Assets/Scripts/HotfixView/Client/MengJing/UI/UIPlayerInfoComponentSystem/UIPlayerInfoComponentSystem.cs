using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIPlayerInfoComponent))]
    [FriendOf(typeof(UIPlayerInfoComponent))]
    public static partial class UIPlayerInfoSystem
    {
        [EntitySystem]
        private static void Awake(this UIPlayerInfoComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Image_PlayerHead = rc.Get<GameObject>("Image_PlayerHead").GetComponent<Image>();
            self.Button_OnPlayerHead = rc.Get<GameObject>("Button_OnPlayerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerCE = rc.Get<GameObject>("Text_PlayerCE").GetComponent<TMP_Text>();
            self.Text_PlayerLianMeng = rc.Get<GameObject>("Text_PlayerLianMeng").GetComponent<TMP_Text>();
            self.Button_AddFriend = rc.Get<GameObject>("Button_AddFriend").GetComponent<Button>();
            self.Button_DeleteFriend = rc.Get<GameObject>("Button_DeleteFriend").GetComponent<Button>();
            self.Button_Report = rc.Get<GameObject>("Button_Report").GetComponent<Button>();
            self.Button_Black = rc.Get<GameObject>("Button_Black").GetComponent<Button>();
            self.Transform_UIFormationSlotItemList = rc.Get<GameObject>("Transform_UIFormationSlotItemList").transform;
            for (int i = 0; i < 9; i++)
            {
                UIFormationSlotItem uiFormationSlotItem = self.AddChild<UIFormationSlotItem, GameObject>(self.Transform_UIFormationSlotItemList
                        .Find(ZString.Format("UIFormationSlotItem_{0}", i + 1)).gameObject);
                self.UIFormationSlotItemList.Add(uiFormationSlotItem);
            }

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIPlayerInfo); });
            self.Button_AddFriend.AddListener(() => { self.OnButton_AddFriend().Coroutine(); });
            self.Button_DeleteFriend.AddListener(() => { self.OnButton_DeleteFriend().Coroutine(); });
            self.Button_Report.AddListener(() => { self.OnButton_Report().Coroutine(); });
            self.Button_Black.AddListener(() => { self.OnButton_Black().Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIPlayerInfoComponent self)
        {
        }

        public static void UpdateInfo(this UIPlayerInfoComponent self, WatchPlayerInfo watchPlayerInfo)
        {
            self.UnitId = watchPlayerInfo.UnitId;
            self.Text_PlayerName.SetText(watchPlayerInfo.PlayerName);
            self.Text_PlayerCE.SetTextFormat("战力:{0}", watchPlayerInfo.CombatPower);

            for (int i = 0; i < 9; i++)
            {
                long heroId = watchPlayerInfo.HeroFormation[i];
                Hero hero = null;

                foreach (HeroInfo heroInfo in watchPlayerInfo.HeroInfoList)
                {
                    if (heroInfo.Id == heroId)
                    {
                        hero = self.AddChildWithId<Hero>(heroInfo.Id);
                        hero.FromMessage(heroInfo);
                    }
                }

                self.UIFormationSlotItemList[i].UpdateInfo(hero, i + 1).Coroutine();
                // self.UIFormationSlotItemList[i].EventTrigger_Click.gameObject.SetActive(false);
            }

            bool isMy = self.UnitId == self.Root().GetComponent<PlayerInfoComponent>().CurrentRoleId;
            if (isMy)
            {
                self.Button_AddFriend.gameObject.SetActive(false);
                self.Button_DeleteFriend.gameObject.SetActive(false);
                self.Button_Report.gameObject.SetActive(false);
                self.Button_Black.gameObject.SetActive(false);
            }
            else
            {
                FriendComponentC friendComponent = self.Root().GetComponent<FriendComponentC>();
                bool isFriend = friendComponent.IsFriend(self.UnitId);
                self.Button_AddFriend.gameObject.SetActive(!isFriend);
                self.Button_DeleteFriend.gameObject.SetActive(isFriend);
            }
        }

        private static async ETTask OnButton_AddFriend(this UIPlayerInfoComponent self)
        {
            int error = await ClientFriendHelper.FriendRequest(self.Root(), self.UnitId);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("申请成功");
            }
        }

        private static async ETTask OnButton_DeleteFriend(this UIPlayerInfoComponent self)
        {
            int error = await ClientFriendHelper.DeleteFriend(self.Root(), self.UnitId);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("删除成功");
            }
        }

        private static async ETTask OnButton_Report(this UIPlayerInfoComponent self)
        {
            int error = await ClientChatHelper.Report(self.Root(), self.UnitId);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("举报成功");
            }
        }

        private static async ETTask OnButton_Black(this UIPlayerInfoComponent self)
        {
            await ETTask.CompletedTask;
        }
    }
}