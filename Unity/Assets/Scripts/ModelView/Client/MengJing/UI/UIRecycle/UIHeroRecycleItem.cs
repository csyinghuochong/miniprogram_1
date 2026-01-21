using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIHeroRecycleItem : Entity, IAwake<GameObject>
    {
        private EntityRef<Hero> hero;
        public Hero Hero { get => this.hero; set => this.hero = value; }

        public GameObject GameObject { get; set; }

        public Image Image_HeroQuality;
        public Image Image_HeroIcon;
        public Image Image_Selected { get; set; }
        public Button Button_Click;
    }
}