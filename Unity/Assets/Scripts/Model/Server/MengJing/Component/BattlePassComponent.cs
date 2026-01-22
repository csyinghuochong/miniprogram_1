namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class BattlePassComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
    }
}