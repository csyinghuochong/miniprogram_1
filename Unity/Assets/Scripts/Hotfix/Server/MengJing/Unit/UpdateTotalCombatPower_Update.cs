using System.Collections.Generic;

namespace ET.Server
{
    [Event(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class UpdateTotalCombatPower_Update : AEvent<Scene, UpdateTotalCombatPower>
    {
        protected override async ETTask Run(Scene scene, UpdateTotalCombatPower args)
        {
            Unit unit = args.Unit;
            long totalCP = 0;

            // 计算总战力

            HeroComponentS heroComponentC = unit.GetComponent<HeroComponentS>();
            List<long> currentFormation = heroComponentC.Formation;

            for (int i = 0; i < currentFormation.Count; i++)
            {
                Hero hero = heroComponentC.GetHero(currentFormation[i]);

                if (hero == null)
                {
                    continue;
                }

                totalCP += hero.NumericDic[NumericType.CombatPower];
            }

            unit.GetComponent<NumericComponentS>().ApplyValue(NumericType.CombatPower, totalCP);

            await ETTask.CompletedTask;
        }
    }
}