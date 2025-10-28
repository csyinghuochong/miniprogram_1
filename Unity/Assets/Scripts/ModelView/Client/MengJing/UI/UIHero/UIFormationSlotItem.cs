using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIFormationSlotItem : Entity, IAwake<GameObject>
    {
        public long HeroId;
        public int SlotIndex;
        public bool IsDrag;
        public GameObject CopyModelGameObject;

        public GameObject GameObject;
        public Transform Transform_HeroIcon;
        public TMP_Text Text_HeroName;
        public EventTrigger EventTrigger_Click;
    }
}