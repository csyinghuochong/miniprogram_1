using TMPro;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UIMainComponent))]
    public class UIMiniMapComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject;

        public TMP_Text Text_MiniMapName;
    }
}