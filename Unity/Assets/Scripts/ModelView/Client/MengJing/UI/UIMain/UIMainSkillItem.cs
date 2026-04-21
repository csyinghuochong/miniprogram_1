using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf(typeof(UIMainSkillComponent))]
    public class UIMainSkillItem : Entity, IAwake<GameObject>, IDestroy
    {
        public long UnitId;
        public int SkillId;
        public long TargetId;
        public long Timer;
        public string AssetsPath;
        public GameObject IndicatorGameObject;

        public GameObject GameObject { get; set; }
        public Image Image_SkillIcon;
        public Image Image_SkillCd;
        public TMP_Text Text_SkillCd;
        public EventTrigger EventTrigger_Click;
        public Image Image_Hp;
        public Image Image_Anger;
    }
}