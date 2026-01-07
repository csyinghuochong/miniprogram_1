namespace ET
{
    [ChildOf]
    public class ArchiveHero : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int HeroConfigId { get; set; }
        public int Star { get; set; }
    }
}