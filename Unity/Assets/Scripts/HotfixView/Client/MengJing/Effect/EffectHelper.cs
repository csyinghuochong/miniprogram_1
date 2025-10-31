using Unity.Mathematics;

namespace ET.Client
{
    public static class EffectHelper
    {
        public static void PlayHitEffect(Unit attack, Unit unit, int skillID)
        {
            //Log.Info("播放受击特效PlayHitEffect:" + skillID);
            //播放受击特效
            if (skillID == 0)
            {
                return;
            }

            SkillConfig skillCof = SkillConfigCategory.Instance.Get(skillID);
            if (skillCof.SkillHitEffectID == 0)
            {
                return;
            }

            if (!EffectConfigCategory.Instance.DataMap.ContainsKey(skillCof.SkillHitEffectID))
            {
                return;
            }

            int angle = 0;

            EffectConfig effectConfig = EffectConfigCategory.Instance.Get(skillCof.SkillHitEffectID);

            EffectData playEffectBuffData = new EffectData();
            playEffectBuffData.EffectId = skillCof.SkillHitEffectID;
            playEffectBuffData.EffectPosition = float3.zero;
            playEffectBuffData.TargetAngle = angle;
            playEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playEffectBuffData);
        }

        public static void PlaySelfEffect(Unit unit, int effectID)
        {
            EffectData playEffectBuffData = new EffectData();
            playEffectBuffData.EffectId = effectID;
            playEffectBuffData.EffectPosition = float3.zero;
            playEffectBuffData.TargetAngle = 0;
            playEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playEffectBuffData);
        }

        public static void PlayEffectPosition(Unit unit, int effectID, float3 position)
        {
            EffectData playEffectBuffData = new EffectData();
            playEffectBuffData.EffectId = effectID;
            playEffectBuffData.EffectPosition = position;
            playEffectBuffData.TargetAngle = 0;
            playEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playEffectBuffData);
        }

        public static void PlayDropEffect(Unit unit, int effectID)
        {
            EffectData playEffectBuffData = new EffectData();
            playEffectBuffData.EffectId = effectID;
            playEffectBuffData.EffectPosition = float3.zero;
            playEffectBuffData.TargetAngle = 0;
            playEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playEffectBuffData);
        }
    }
}