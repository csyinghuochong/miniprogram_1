

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawComponent))]
    [FriendOf(typeof(UILotteryDrawComponent))]
    public static partial class UILotteryDrwaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_Type_LotteryTicket = rc.Get<GameObject>("Text_Type_LotteryTicket").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
            self.Button_AddDiamond = rc.Get<GameObject>("Button_AddDiamond").GetComponent<Button>();
            self.Button_RewardPreview = rc.Get<GameObject>("Button_RewardPreview").GetComponent<Button>();
            self.Button_Probability = rc.Get<GameObject>("Button_Probability").GetComponent<Button>();
            self.Button_Wish = rc.Get<GameObject>("Button_Wish").GetComponent<Button>();
            self.Text_BaoDiTips = rc.Get<GameObject>("Text_BaoDiTips").GetComponent<TMP_Text>();
            self.Button_DrawOne = rc.Get<GameObject>("Button_DrawOne").GetComponent<Button>();
            self.Button_DrawTen = rc.Get<GameObject>("Button_DrawTen").GetComponent<Button>();
            self.Text_FreeTime = rc.Get<GameObject>("Text_FreeTime").GetComponent<TMP_Text>();
            self.Toggle_SkipAnimation = rc.Get<GameObject>("Toggle_SkipAnimation").GetComponent<Toggle>();
            self.GameObject_RewardPreview = rc.Get<GameObject>("GameObject_RewardPreview");
            self.GameObject_Probability = rc.Get<GameObject>("GameObject_Probability");
            self.GameObject_Wish = rc.Get<GameObject>("GameObject_Wish");

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UILotteryDraw); });

        }
    }
}