using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIHeroRecycleItem : Entity, IAwake<GameObject>
    {
        public GameObject GameObject { get; set; }
        
        public Image Image_HeroQuality;
        public Image Image_HeroIcon;
        public Image Image_Selected;
        public Button Button_Click;
    }
}