using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class UIPlayerHpComponent : Entity, IAwake, IDestroy
    {
        public string HeadBarPath;

        public GameObject GameObject { get; set; }
        public TMP_Text Text_Name;
    }
}