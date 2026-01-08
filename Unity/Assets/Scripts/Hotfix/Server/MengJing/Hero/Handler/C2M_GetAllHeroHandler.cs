using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponent))]
    public class C2M_GetAllHeroHandler : MessageLocationHandler<Unit, C2M_GetAllHero, M2C_GetAllHero>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllHero request, M2C_GetAllHero response)
        {
            HeroComponent heroComponent = unit.GetComponent<HeroComponent>();

            foreach (var hero in heroComponent.GetAllHero())
            {
                response.HeroList.Add(hero.ToMessage());
            }

            response.Formation.AddRange(heroComponent.Formation);

            await ETTask.CompletedTask;
        }
    }
}