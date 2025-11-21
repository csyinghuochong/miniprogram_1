using System.Collections.Generic;

namespace ET.Server
{
    public class Buff_生命图腾 : BuffSHandler
    {
        public override void OnInit(BuffS buff)
        {
        }

        public override void OnUpdate(BuffS buff, float deltaTime)
        {
            buff.RunTime += deltaTime;

            if (buff.RunTime >= buff.BuffEndTime)
            {
                buff.BuffState = BuffState.Finished;
                return;
            }

            if (buff.RunTime >= buff.BuffConfig.BuffDelayTime)
            {
                // 循环触发
                if (buff.BuffConfig.BuffLoopTime > 0)
                {
                    float timeSinceDelay = buff.RunTime - buff.BuffConfig.BuffDelayTime;
                    int expectedTriggerCount = (int)(timeSinceDelay / buff.BuffConfig.BuffLoopTime);

                    if (expectedTriggerCount > buff.TriggerCount)
                    {
                        buff.TriggerCount = expectedTriggerCount;

                        switch (buff.BuffConfig.BuffType)
                        {
                            // 属性
                            case 1:
                            {
                                List<EntityRef<Unit>> entities = buff.TheUnitBelongTo.GetParent<UnitComponent>().GetAll();
                                for (int i = entities.Count - 1; i >= 0; i--)
                                {
                                    Unit defendUnit = entities[i];

                                    if (!UnitHelper.IsTeam(buff.TheUnitBelongTo, defendUnit))
                                    {
                                        continue;
                                    }

                                    NumericComponentS numericComponent = defendUnit.GetComponent<NumericComponentS>();
                                    if (numericComponent == null)
                                    {
                                        continue;
                                    }

                                    int type = buff.BuffConfig.BuffParameterType;
                                    long value = buff.BuffConfig.BuffParameterValue;
                                    // x属性值
                                    if (buff.BuffConfig.BuffParameterValueType != 0)
                                    {
                                        value = (long)(numericComponent.GetAsLong(buff.BuffConfig.BuffParameterValueType) * (value / 10000f));
                                    }

                                    if (value != 0)
                                    {
                                        if (type == NumericType.Now_Hp)
                                        {
                                            numericComponent.ApplyChange(type, value, true, true, buff.TheUnitFrom.Id, buff.InitBuffData.SkillConfigId, DamageType.Recover);
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        public override void OnFinished(BuffS buff)
        {
        }
    }
}