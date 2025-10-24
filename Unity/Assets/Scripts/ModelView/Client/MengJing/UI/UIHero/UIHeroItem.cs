using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIHeroItem : Entity, IAwake<GameObject>
    {
        public long HeroId;

        public GameObject GameObject { get; set; }

        public TMP_Text Text_HeroName;
        public Image Image_HeroIcon;
        public Transform Transform_HeroStar;
        public TMP_Text Text_HeroCombatPower;
        public Button Button_Click;
    }
}