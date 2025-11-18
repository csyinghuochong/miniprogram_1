using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UIMainComponent))]
    public class UIMainSkillComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject;
        public Transform Transform_SkillList;
        public GameObject UIMainSkillItem;

        public long Timer;
        public List<UIMainSkillItem> UIMainSkillItemList { get; set; } = new();
    }
}