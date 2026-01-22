namespace ET.Client
{
    [EntitySystemOf(typeof(BattlePassComponentC))]
    public static partial class BattlePassComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this BattlePassComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BattlePassComponentC self)
        {
        }
    }
}