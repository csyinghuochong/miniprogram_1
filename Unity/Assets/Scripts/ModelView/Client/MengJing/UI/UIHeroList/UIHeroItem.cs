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

        public Image Image_HeroIcon;
        public TMP_Text Text_HeroName;
    }
}