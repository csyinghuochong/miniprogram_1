namespace ET
{
    public interface INumericWatcher
    {
        void Run(Unit unit, NumbericChange args);
    }

    public struct NumbericChange
    {
        public Unit Defend;
        public long AttackId;
        public int NumericType;
        public long OldValue;
        public long NewValue;
        public int SkillId;
        public DamageType DamageType;
    }

    public enum DamageType
    {
        None = 0,
        Physical = 1, //物理伤害
        Magic = 2, //法术伤害
        Critical = 3, //暴击
        Dodge = 4, //闪避
        Recover = 5, //恢复
        Immune = 6, //免疫
    }
}