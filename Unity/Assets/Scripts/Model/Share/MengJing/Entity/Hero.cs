namespace ET
{
    public enum HeroOpType
    {
        Add,
        Remove,
        Update,
    }

    public enum HeroType
    {
        Warrior = 1, //战士
        Archer = 2, //射手
        Mage = 3, //法师
    }

    [ChildOf]
    public class Hero : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int Lv { get; set; }
        public int Star { get; set; }
    }
}