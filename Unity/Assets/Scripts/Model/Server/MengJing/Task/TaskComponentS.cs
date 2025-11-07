using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class TaskComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<TaskPro>> TaskPros = new();

        public List<int> CompleteTasks { get; set; } = new();
    }
}