namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class ArchiveComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
    }
}