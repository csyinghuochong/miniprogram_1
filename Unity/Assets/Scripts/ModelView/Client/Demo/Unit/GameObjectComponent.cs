using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class GameObjectComponent : Entity, IAwake, IDestroy
    {
        public string UnitAssetsPath { get; set; }
        public GameObject GameObject { get; set; }
        public SkeletonAnimation SkeletonAnimation { get; set; }

        public Vector3 LastPosition = Vector3.zero;
    }
}