using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIMainCityMapComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public List<UIMainCityMapNPCButton> UIMainCityMapNPCButtonList { get; set; } = new();

        public Button Button_Close;
        public RawImage RawImage_Map;
        public Transform Transform_HeadList;
        public Transform Transform_HeadItem;
        public GameObject MapCamera;
        public Transform Content_UIMainCityMapNPCButton;
        public GameObject UIMainCityMapNPCButton;
        
        public float ScaleRateX;
        public float ScaleRateY;
        public Vector2 LocalPoint;
        public Dictionary<long, GameObject> AllPointList = new();
        public List<GameObject> CachePointList = new();
    }
}