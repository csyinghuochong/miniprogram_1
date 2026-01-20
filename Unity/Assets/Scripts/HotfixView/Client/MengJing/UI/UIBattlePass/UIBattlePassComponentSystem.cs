using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class DataUpdate_UpdateUserData_UIBattlePassRefresh : AEvent<Scene, UpdateUserData>
    {
        protected override async ETTask Run(Scene scene, UpdateUserData args)
        {
            if (args.UserDataType != UserDataType.Gold && args.UserDataType != UserDataType.Diamond)
            {
                return;
            }

            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIBattlePass);
            if (ui == null)
            {
                return;
            }

            UIBattlePassComponent uiBattlePassComponent = ui.GetComponent<UIBattlePassComponent>();
            if (args.UserDataType == UserDataType.Gold)
            {
                uiBattlePassComponent.UpdateGold();
            }

            if (args.UserDataType == UserDataType.Diamond)
            {
                uiBattlePassComponent.UpdateDiamond();
            }

            await ETTask.CompletedTask;
        }
    }
    
    [EntitySystemOf(typeof(UIBattlePassComponent))]
    [FriendOf(typeof(UIBattlePassComponent))]
    public static partial class UIBattlePassComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIBattlePassComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Button_AddGold = rc.Get<GameObject>("Button_AddGold").GetComponent<Button>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
            self.Button_AddDiamond = rc.Get<GameObject>("Button_AddDiamond").GetComponent<Button>();
            self.Content_UIBattlePassItem = rc.Get<GameObject>("Content_UIBattlePassItem").transform;
            self.UIBattlePassItem = rc.Get<GameObject>("UIBattlePassItem");
            self.UIBattlePassItem.gameObject.SetActive(false);
            self.Button_GetAllReward = rc.Get<GameObject>("Button_GetAllReward").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIBattlePass); });
            
            self.UpdateGold();
            self.UpdateDiamond();
        }

        [EntitySystem]
        private static void Destroy(this UIBattlePassComponent self)
        {
            self.UIBattlePassItemList.Clear();
            self.UIBattlePassItem = null;
        }
        
        public static void UpdateGold(this UIBattlePassComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();

            if (userInfoComponent.Gold >= 10000)
            {
                self.Text_Type_Gold.SetTextFormat("{0}k", userInfoComponent.Gold / 1000);
                return;
            }

            self.Text_Type_Gold.SetText(userInfoComponent.Gold);
        }

        public static void UpdateDiamond(this UIBattlePassComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            
            if (userInfoComponent.Diamond >= 10000)
            {
                self.Text_Type_Diamond.SetTextFormat("{0}k", userInfoComponent.Diamond / 1000);
                return;
            }

            self.Text_Type_Diamond.SetText(userInfoComponent.Diamond);
        }

    }
}