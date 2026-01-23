using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UILotteryDrawComponent : Entity, IAwake
    {
        public int WishItemId;
        public UILotteryDrawRewardPreviewComponent UILotteryDrawRewardPreviewComponent { get; set; }
        public UILotteryDrawProbabilityComponent UILotteryDrawProbabilityComponent { get; set; }
        public UILotteryDrawWishComponent UILotteryDrawWishComponent { get; set; }

        public Button Button_Close;
        public Button Button_RewardPreview;
        public Button Button_Probability;
        public Button Button_Wish;
        public TMP_Text Text_BaoDiTips;
        public Button Button_DrawOne;
        public Button Button_DrawTen;
        public TMP_Text Text_FreeTime;
        public Toggle Toggle_SkipAnimation;
        public Image Image_WishIcon;
    }
}