using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIAchievementItem : Entity, IAwake<GameObject>
    {
        public int AchieveConfigId;

        public GameObject GameObject { get; set; }

        public Image Image_Icon;
        public TMP_Text Text_Name;
        public TMP_Text Text_Description;
        public TMP_Text Text_RewardPoints;
        public TMP_Text Text_Progress;
        public GameObject GameObject_Completed;
    }
}