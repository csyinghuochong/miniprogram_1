using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIMainCityMapNPCButton : Entity, IAwake<GameObject>
    {
        public GameObject GameObject { get; set; }

        public Button Button_GoToNPC;
        public TMP_Text Text_NPCName;
    }
}