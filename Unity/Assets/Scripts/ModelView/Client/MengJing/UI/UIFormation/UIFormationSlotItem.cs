using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIFormationSlotItem : Entity, IAwake<GameObject>
    {
        public long HeroId;
        
        public GameObject GameObject;
        public Image Image_HeroIcon;
        public TMP_Text Text_HeroName;
        public Button Button_Click;
    }
}