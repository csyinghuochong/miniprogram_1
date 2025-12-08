using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class PickUpDropItemComponent : Entity, IAwake, IUpdate, IDestroy
    {
        private EntityRef<Unit> mainUnit;
        public Unit MainUnit { get => this.mainUnit; set => this.mainUnit = value; }
        public List<EntityRef<Unit>> DropItemList = new();
        
        public long LastSendTime = 0;
        public List<long> SendIdList = new();
    }
}