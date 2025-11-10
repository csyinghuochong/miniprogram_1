using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class InventoryComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        /// <summary>
        /// 按容器类型分类存储道具，key是ContainerType，value是该容器中的道具列表
        /// </summary>
        [BsonIgnore]
        public Dictionary<int, List<EntityRef<Item>>> ItemsByContainer = new();
    }
}