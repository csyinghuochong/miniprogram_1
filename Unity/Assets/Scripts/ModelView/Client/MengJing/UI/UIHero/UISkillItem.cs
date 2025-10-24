using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UISkillItem : Entity, IAwake<GameObject>, IDestroy
    {
        public int SkillConfigId;

        public GameObject GameObject { get; set; }
        public Image Image_SkillIcon;
        public Button Button_Click;
        public GameObject Unlock;
        public TMP_Text Text_Unlock;
    }
}