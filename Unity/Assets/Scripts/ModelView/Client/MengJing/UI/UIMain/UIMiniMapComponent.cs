using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UIMainComponent))]
    public class UIMiniMapComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject;
        public RawImage RawImage_Map;
        public TMP_Text Text_MiniMapName;
        public Transform Transform_HeadList;
        public Transform Transform_HeadItem;
        public GameObject MapCamera;
        
        public float ScaleRateX;
        public float ScaleRateY;
        public Dictionary<long, GameObject> AllPointList = new();
        public List<GameObject> CachePointList = new();	
    }
}