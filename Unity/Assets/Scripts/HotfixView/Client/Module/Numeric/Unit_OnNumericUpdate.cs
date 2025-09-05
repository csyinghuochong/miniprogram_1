namespace ET.Client
{
    
    /// <summary>
    /// 表现
    /// </summary>
    ///
    ///
    /// 
    [Event(SceneType.Current)]
    public class Unit_OnNumericUpdate : AEvent<Scene, NumbericChange>
    {
        protected override async ETTask Run(Scene scene, NumbericChange args)
        {
            Scene root = scene.Root();
            switch (args.NumericType)
            {
                case NumericType.Now_Hp:
                case NumericType.Now_MaxHp:
                    NumericComponentC numericComponentDefend = args.Defend.GetComponent<NumericComponentC>();
                    long costHp = 0;
                    if (args.NumericType == NumericType.Now_Hp)
                    {
                        long nowHpValue = numericComponentDefend.GetAsLong(NumericType.Now_Hp);
                        costHp = (nowHpValue - args.OldValue);
                    }

                    // EventSystem.Instance.Publish(args.Defend.Root(), new Now_Hp_Update()
                    // {
                    //     Defend = args.Defend,
                    //     ChangeHpValue = costHp,
                    //     DamgeType = args.DamgeType,
                    //     SkillID = args.SkillId,
                    //     AttackId = args.AttackId
                    // });
                    break;
                default:
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}