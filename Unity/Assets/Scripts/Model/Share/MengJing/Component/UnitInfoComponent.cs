namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class UnitInfoComponent : Entity, IAwake, ITransfer, IDestroy
    {
        public string UnitName { get; set; } //自身名字
        public string MasterName { get; set; } //主人名字
    }
}