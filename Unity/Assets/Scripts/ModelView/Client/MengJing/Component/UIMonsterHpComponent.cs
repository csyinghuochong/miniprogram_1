using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class UIMonsterHpComponent : Entity, IAwake, IDestroy
    {
        public string HeadBarPath;

        public GameObject GameObject { get; set; }
        public Image Image_Hp;
    }
}