using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class GameObjectComponent: Entity, IAwake, IDestroy
    {
        public string UnitAssetsPath { get; set; }
        public GameObject GameObject { get; set; }
        public string HorseAssetsPath;
        public GameObject ObjectHorse;
        
    }
}