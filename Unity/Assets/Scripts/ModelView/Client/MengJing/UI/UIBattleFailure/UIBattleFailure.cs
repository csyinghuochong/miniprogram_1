using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIBattleFailureComponent : Entity, IAwake
    {
        public Button Button_Rechallenge;
        public Button Button_Recall;
    }
}