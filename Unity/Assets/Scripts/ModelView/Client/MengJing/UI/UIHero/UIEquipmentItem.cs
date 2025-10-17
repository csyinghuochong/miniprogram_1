using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIEquipmentItem : Entity, IAwake<GameObject>, IDestroy
    {
        public long HeroId { get; set; }
        public EquipSlotType EquipSlotType { get; set; }

        public GameObject GameObject;
        public Image Image_ItemQuality;
        public Image Image_ItemIcon;
        public Button Button_Click;
    }
}