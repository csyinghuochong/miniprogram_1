using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UIMainComponent))]
    public class UIMainSkillComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }

        public List<UIMainSkillItem> UIMainSkillItemList { get; set; } = new();
    }
}