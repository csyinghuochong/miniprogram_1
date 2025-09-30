using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GetAllHeroHandler : MessageLocationHandler<Unit, C2M_GetAllHero, M2C_GetAllHero>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllHero request, M2C_GetAllHero response)
        {
            HeroComponentS heroComponentS = unit.GetComponent<HeroComponentS>();

            foreach (var hero in heroComponentS.GetAllHeros())
            {
                response.HeroList.Add(hero.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}