namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_HeroUseSkillHandler : MessageLocationHandler<Unit, C2M_HeroUseSkill, M2C_HeroUseSkill>
    {
        protected override async ETTask Run(Unit unit, C2M_HeroUseSkill request, M2C_HeroUseSkill response)
        {
            MapComponent mapComponent = unit.Scene().GetComponent<MapComponent>();

            if (mapComponent.MapType != MapType.LocalLevel)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            Unit hero = unit.GetParent<UnitComponent>().Get(request.HeroUnitId);
            if (hero == null)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            response.Error = hero.GetComponent<SkillManagerComponentS>().TryUseSkill(request.SkillConfigId, request.TargetId, request.Angle, request.Position);

            await ETTask.CompletedTask;
        }
    }
}