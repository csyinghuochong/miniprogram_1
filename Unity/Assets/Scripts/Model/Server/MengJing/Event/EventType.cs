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
        public StateType nowStateType;
        public string stateValue;
    }

    public struct StateTypeRemove
    {
        public Unit UnitDefend;
        public StateType nowStateType;
        public string stateValue;
    }

    public struct TriggerTask
    {
        public Unit Unit;
        public TaskTargetType TargetType;
        public int TargetId;
        public int TargetValue;
    }

    public struct UpdateTotalCombatPower
    {
        public Unit Unit;
    }

    public struct AddOrUpdateHero
    {
        public Unit Unit;
        public Hero Hero;
    }
}