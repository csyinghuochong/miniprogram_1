namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class StateComponent : Entity, IAwake, ITransfer, IDeserialize
    {
        public StateType CurrentStateType { get; set; }
        public long RigidityEndTime { get; set; }
        public long NetWaitEndTime { get; set; }
    }
}