using UnityEngine;

namespace ET.Client
{
    public static class PosType
    {
        public const string Hp = "Hp";
        public const string Center = "Center";
        public const string Bottom = "Bottom";
    }

    [ComponentOf(typeof(Unit))]
    public class UnitBoneComponent : Entity, IAwake, IDestroy
    {
        public Transform Hp { get; set; }
        public Transform Center { get; set; }
        public Transform Bottom { get; set; }
    }
}