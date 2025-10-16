using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UITeamItem : Entity, IAwake<GameObject>
    {
        public long HeroId;

        public GameObject GameObject { get; set; }
        public Image Image_HeroIcon;
        public TMP_Text Text_Lv;
        public TMP_Text Text_Name;
        public TMP_Text Text_CP;
        public Button Button_Up;
    }
}