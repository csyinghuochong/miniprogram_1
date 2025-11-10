using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class InventoryComponentC : Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 按容器类型分类存储道具，key是ContainerType，value是该容器中的道具列表
        /// </summary>
        public Dictionary<int, List<EntityRef<Item>>> ItemsByContainer = new();
    }
}