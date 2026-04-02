using Unity.Mathematics;

namespace ET.Client
{
    public static class ClientSkillHelper
    {
        public static async ETTask<int> HeroUseSkill(Scene root, long heroUnitId, int skillConfigId, long targetId, float angle, float3 position)
        {
            C2M_HeroUseSkill request = C2M_HeroUseSkill.Create(true);
            request.HeroUnitId = heroUnitId;
            request.SkillConfigId = skillConfigId;
            request.TargetId = targetId;
            request.Angle = angle;
            request.Position = position;

            using M2C_HeroUseSkill response = (M2C_HeroUseSkill)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response.Error;
        }

        public static float GetCenterHigh(Unit unit)
        {
            if (unit.Type == UnitType.Hero)
            {
                return HeroConfigCategory.Instance.Get(unit.ConfigId).CenterY;
            }

            if (unit.Type == UnitType.Monster)
            {
                return MonsterConfigCategory.Instance.Get(unit.ConfigId).CenterY;
            }

            return 1f;
        }
    }
}