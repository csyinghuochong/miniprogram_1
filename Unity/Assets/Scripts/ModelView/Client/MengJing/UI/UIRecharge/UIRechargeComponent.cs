

using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIRechargeComponent : Entity, IAwake
    {
        public UIRechargePointsRewardComponent UIRechargePointsRewardComponent { get; set; }
        
        public Button Button_Close;
        public Button Button_RechargeOne;
        public Button Button_RechargeTwo;
        public Button Button_RechargeThree;
        public Button Button_RechargeFour;
        public Button Button_RechargeFive;
        public Button Button_RechargeSix;
        public TMP_Text Text_VipLv;
        public Image Image_PointsProgress;
        public Button Button_Reward;
    }
}