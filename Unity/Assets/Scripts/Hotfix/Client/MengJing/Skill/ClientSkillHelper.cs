using Unity.Mathematics;

namespace ET.Client
{
    public static class ClientSkillHelper
    {
        public static async ETTask<int> HeroUseSkill(Scene root, long heroUnitId, int skillConfigId, long targetId, float angle, float3 position)
        {
            C2M_HeroUseSkill request = C2M_HeroUseSkill.Create();
            request.HeroUnitId = heroUnitId;
            request.SkillConfigId = skillConfigId;
            request.TargetId = targetId;
            request.Angle = angle;
            request.Position = position;

            M2C_HeroUseSkill response = (M2C_HeroUseSkill)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }
    }
}