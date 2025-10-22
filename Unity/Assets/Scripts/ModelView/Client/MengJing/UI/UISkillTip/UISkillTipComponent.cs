using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UISkillTipComponent : Entity, IAwake
    {
        public Button Button_Close;
        public Image Image_SkillIcon;
        public TMP_Text Text_SkillName;
        public TMP_Text Text_SkillDes;
    }
}