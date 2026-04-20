using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class PickUpDropItemComponent : Entity, IAwake, IDestroy
    {
        private EntityRef<Unit> mainUnit;
        public Unit MainUnit { get => this.mainUnit; set => this.mainUnit = value; }
        public List<EntityRef<Unit>> DropItemList = new();
        
        public long LastSendTime = 0;
        public List<long> SendIdList = new();
        public long Timer;

        public const float PickUpRange = 8f;
    }
}