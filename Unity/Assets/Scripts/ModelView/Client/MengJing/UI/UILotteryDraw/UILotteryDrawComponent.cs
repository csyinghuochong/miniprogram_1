using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UILotteryDrawComponent : Entity, IAwake
    {
        public Button Button_Close;
        public TMP_Text Text_Type_LotteryTicket;
        public TMP_Text Text_Type_Diamond;
        public Button Button_AddDiamond;
        public Button Button_RewardPreview;
        public Button Button_Probability;
        public Button Button_Wish;
        public TMP_Text Text_BaoDiTips;
        public Button Button_DrawOne;
        public Button Button_DrawTen;
        public TMP_Text Text_FreeTime;
        public Toggle Toggle_SkipAnimation;
        public GameObject GameObject_RewardPreview;
        public GameObject GameObject_Probability;
        public GameObject GameObject_Wish;
    }
}