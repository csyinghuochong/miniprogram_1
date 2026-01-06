using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIArchiveHeroItem : Entity, IAwake<GameObject>
    {
        public long HeroId;
        public int HeroConfigId;

        public GameObject GameObject { get; set; }

        public Image Image_HeroQuality;
        public TMP_Text Text_HeroName;
        public Image Image_HeroIcon;
        public Transform Transform_HeroStar;
        public Button Button_Click;
        public Button Button_JiFen;
        public TMP_Text Text_JiFen;
        public TMP_Text Text_NotHave;
    }
}