using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class C2M_GetAllHeroHandler : MessageLocationHandler<Unit, C2M_GetAllHero, M2C_GetAllHero>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllHero request, M2C_GetAllHero response)
        {
            HeroComponentS heroComponentS = unit.GetComponent<HeroComponentS>();

            foreach (var hero in heroComponentS.GetAllHero())
            {
                response.HeroList.Add(hero.ToMessage());
            }

            response.CurrentFormationIndex = heroComponentS.CurrentFormationIndex;
            response.Formation_1 = heroComponentS.Formation_1;
            response.Formation_2 = heroComponentS.Formation_2;

            await ETTask.CompletedTask;
        }
    }
}