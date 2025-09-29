using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIGMComponent: Entity, IAwake
    {
        public Button Button_Close;
        public TMP_InputField InputField_AddItem_ItemId;
        public TMP_InputField InputField_AddItem_ItemNum;
        public Button Button_AddItem_Send;
    }
}