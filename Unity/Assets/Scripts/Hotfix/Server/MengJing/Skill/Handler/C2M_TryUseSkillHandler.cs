namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_TryUseSkillHandler : MessageLocationHandler<Unit, C2M_TryUseSkill, M2C_TryUseSkill>
    {
        protected override async ETTask Run(Unit unit, C2M_TryUseSkill request, M2C_TryUseSkill response)
        {
            response.Error = unit.GetComponent<SkillManagerComponentS>().TryUseSkill(request.SkillConfigId, request.TargetId, request.Angle, request.Position);

            await ETTask.CompletedTask;
        }
    }
}