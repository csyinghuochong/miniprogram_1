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
        public Image Image_Border;
        public Image Image_HeroIcon;
        public Button Button_Click;
    }
}