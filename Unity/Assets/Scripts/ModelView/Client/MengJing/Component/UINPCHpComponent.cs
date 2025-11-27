using TMPro;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class UINPCHpComponent : Entity, IAwake, IDestroy
    {
        public string HeadBarPath;

        public GameObject GameObject { get; set; }
        public TMP_Text Text_Name;
    }
}