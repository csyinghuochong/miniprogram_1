using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UIMainComponent))]
    public class UIMainSkillComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject;
        public Button Button_AutoFight;
        public Transform Transform_SkillList;
        public GameObject UIMainSkillItem;

        public bool AutoFight { get; set; }
        public long Timer;
        public List<UIMainSkillItem> UIMainSkillItemList { get; set; } = new();
    }
}