using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroAttributesComponent : Entity, IAwake, IDestroy
    {
        public long CurrentHeroId { get; set; }
        
        public Button Button_Close;
        public Transform Content_UIBaseAttributeItem;
        public GameObject UIBaseAttributeItem;
        public Transform Content_UIOtherAttributeItem;
        public GameObject UIOtherAttributeItem;
        
    }
}