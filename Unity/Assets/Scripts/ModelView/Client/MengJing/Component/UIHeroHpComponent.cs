using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class UIHeroHpComponent : Entity, IAwake, IDestroy
    {
        public string HeadBarPath;

        public GameObject GameObject { get; set; }
        public TMP_Text Text_Name;
        public Image Image_Hp;
    }
}