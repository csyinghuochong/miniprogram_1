using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UIMainComponent))]
    public class UIMiniMapComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject;

        public TMP_Text Text_MiniMapName;
        public RawImage RawImage_Map;
        public GameObject MapCamera;
    }
}