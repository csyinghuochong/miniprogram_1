using UnityEngine;

namespace ET.Client
{
    public static class PosType
    {
        public const string Hp = "Hp";
        public const string Center = "Center";
    }

    [ComponentOf(typeof(Unit))]
    public class UnitBoneComponent : Entity, IAwake, IDestroy
    {
        public Transform Hp { get; set; }
        public Transform Center { get; set; }
    }
}