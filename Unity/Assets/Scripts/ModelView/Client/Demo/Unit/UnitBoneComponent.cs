using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class UnitBoneComponent : Entity, IAwake, IDestroy
    {
        public Transform Hp { get; set; }
    }
}