using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UITeamComponent : Entity, IAwake
    {
        public Button Button_Close;
    }
}