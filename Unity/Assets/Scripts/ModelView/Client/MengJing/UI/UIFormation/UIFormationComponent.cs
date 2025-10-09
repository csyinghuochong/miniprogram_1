using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIFormationComponent : Entity, IAwake
    {
        public UIFormationSlotItem UIFormationSlotItem_1 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_2 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_3 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_4 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_5 { get; set; }

        public Button Button_Close;
        public Button Button_Plan_1;
        public Button Button_Plan_2;
    }
}