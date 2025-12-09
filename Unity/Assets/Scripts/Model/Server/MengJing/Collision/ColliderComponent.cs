using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Unity.Mathematics;

namespace ET.Server
{
    public enum ColliderType
    {
        Static,
        Dynamic,
        Kinematic
    }

    [ComponentOf(typeof(Unit))]
    public class ColliderComponent : Entity, IAwake<Unit, ColliderType>, IUpdate, IDestroy
    {
        private EntityRef<Unit> belongToUnit;
        public Unit BelongToUnit { get => this.belongToUnit; set => this.belongToUnit = value; }

        private EntityRef<Unit> parentUnit;
        public Unit ParentUnit { get => this.parentUnit; set => this.parentUnit = value; }

        public Body Body;

        public string CollisionHandler { get; set; }
    }
}