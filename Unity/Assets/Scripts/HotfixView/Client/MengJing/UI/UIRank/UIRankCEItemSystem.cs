using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRankCEItem))]
    [FriendOf(typeof(UIRankCEItem))]
    public static partial class UIRankCEItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIRankCEItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_PlayerHead = rc.Get<GameObject>("Image_PlayerHead").GetComponent<Image>();
            self.Button_OnPlayerHead = rc.Get<GameObject>("Button_OnPlayerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerCE = rc.Get<GameObject>("Text_PlayerCE").GetComponent<TMP_Text>();
            self.Text_Sort = rc.Get<GameObject>("Text_Sort").GetComponent<TMP_Text>();

            self.Button_OnPlayerHead.AddListener(() => self.OnButton_OnPlayerHead().Coroutine());
        }

        public static void UpdateInfo(this UIRankCEItem self, RankData rankData)
        {
            self.UnitId = rankData.UnitId;

            self.Text_Sort.SetText(rankData.Rank);
            self.Text_PlayerName.SetText(rankData.PlayerName);

            long combatPower = rankData.CombatPower;
            if (combatPower >= 10000)
            {
                combatPower = combatPower / 10000;
                self.Text_PlayerCE.SetTextFormat("战力:{0}万", combatPower);
            }
            else
            {
                self.Text_PlayerCE.SetTextFormat("战力:{0}", combatPower);
            }
        }

        private static async ETTask OnButton_OnPlayerHead(this UIRankCEItem self)
        {
            M2C_WatchPlayer response = await ClientUserInfoHelper.WatchPlayer(self.Root(), self.UnitId);

            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIPlayerInfo);
            UIPlayerInfoComponent uiPlayerInfoComponent = ui.GetComponent<UIPlayerInfoComponent>();
            uiPlayerInfoComponent.UpdateInfo(response.WatchPlayerInfo);
        }
    }
}