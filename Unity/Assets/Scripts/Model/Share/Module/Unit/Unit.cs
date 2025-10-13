using System.Diagnostics;
using MongoDB.Bson.Serialization.Attributes;
using Unity.Mathematics;

namespace ET
{
    [ChildOf(typeof(UnitComponent))]
    [DebuggerDisplay("ViewName,nq")]
    public partial class Unit : Entity, IAwake<int>
    {
        public int AI { get; set; }

        public int Type { get; set; }

        public int ConfigId { get; set; } //配置表id

        public bool MainHero { get; set; }

        public int SceneType { get; set; }

        public int SpeedRate { get; set; }

        [BsonIgnore]
        public bool WaitLoad { get; set; }

        public float3 Position { get; set; }

        [BsonIgnore]
        public float3 Forward
        {
            get => math.mul(this.Rotation, math.forward());
            set => this.Rotation = quaternion.LookRotation(value, math.up());
        }

        public quaternion Rotation { get; set; }

        protected override string ViewName
        {
            get
            {
                return $"{this.GetType().FullName} ({this.Id})";
            }
        }
    }
}