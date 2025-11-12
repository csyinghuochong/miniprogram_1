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
        Normal = 1, //普通伤害
        Critical = 2, //暴击
        Dodge = 3, //闪避
        Recover = 4 //恢复
    }
}