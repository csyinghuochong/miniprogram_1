using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIBattlePassItem : Entity, IAwake<GameObject>
    {
        public int RewardId;
        public UICommonItem UICommonItem_1 { get; set; }
        public UICommonItem UICommonItem_2 { get; set; }
        public UICommonItem UICommonItem_3 { get; set; }
        public GameObject GameObject { get; set; }

        public TMP_Text Text_RequiredLv;
        public Transform Transform_Reward1;
        public Transform Transform_Reward2;
        public Transform Transform_Reward3;
        public GameObject UICommonItem;
        public GameObject GameObject_NotCompleted;
        public Button Button_OnClick;
    }
}