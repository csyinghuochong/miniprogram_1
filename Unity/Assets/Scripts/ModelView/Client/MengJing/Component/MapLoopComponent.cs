using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MapLoopComponent : Entity, IAwake, IUpdate, IDestroy
    {
        private EntityRef<Unit> lookAtUnit;
        public Unit LookAtUnit { get => this.lookAtUnit; set => this.lookAtUnit = value; }

        public List<GameObject> MapList = new();
        public float TotalHeight;
    }
}