using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIBagComponent : Entity, IAwake
    {
        public Button Button_Close;
    }
}