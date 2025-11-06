namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class StateComponentC : Entity, IAwake
    {
        public long CurrentStateType;
    }
}