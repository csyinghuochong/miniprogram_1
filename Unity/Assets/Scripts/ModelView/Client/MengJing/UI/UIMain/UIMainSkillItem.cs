using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UIMainSkillComponent))]
    public class UIMainSkillItem : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
    }
}