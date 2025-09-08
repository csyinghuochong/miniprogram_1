namespace ET.Server
{
  
    public struct GMCommonRequest
    {
        public string Context;
    }
    
    public struct UnitKillEvent
    {
        public int WaitRevive;
        public Unit UnitAttack;
        public Unit UnitDefend;
        public bool NoDrop;
    }
    
    public struct StateTypeAdd
    {
        public Unit UnitDefend;
        public long nowStateType;
        public string stateValue;
    }

    public struct StateTypeRemove
    {
        public Unit UnitDefend;
        public long nowStateType;
        public string stateValue;
    }

    public struct GenerateSerials
    {
        public Scene Scene;
    }
    
}