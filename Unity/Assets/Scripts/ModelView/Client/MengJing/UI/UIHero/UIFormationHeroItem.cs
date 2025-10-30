using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIFormationHeroItem : Entity, IAwake<GameObject>
    {
        public long HeroId;

        public GameObject GameObject { get; set; }
        public Image Image_HeroIcon;
        public Image Image_Selected;
        public TMP_Text Text_Lv;
        public TMP_Text Text_HeroCP;
        public Button Button_Click;
    }
}