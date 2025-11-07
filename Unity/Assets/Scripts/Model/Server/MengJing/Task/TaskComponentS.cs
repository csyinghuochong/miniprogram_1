using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class TaskComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<TaskPro>> TaskProList = new();

        public List<int> CompleteTaskList { get; set; } = new();
    }
}