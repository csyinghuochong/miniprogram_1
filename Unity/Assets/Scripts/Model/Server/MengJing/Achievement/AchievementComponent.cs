namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class AchievementComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
    }
}