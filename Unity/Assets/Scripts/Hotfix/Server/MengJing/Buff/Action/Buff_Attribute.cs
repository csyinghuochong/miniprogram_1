namespace ET.Server
{
    public class Buff_Attribute : BuffSHandler
    {
        public override void OnInit(BuffS buff)
        {
        }

        public override void OnUpdate(BuffS buff, float deltaTime)
        {
            NumericComponentS numericComponent = buff.TheUnitBelongTo.GetComponent<NumericComponentS>();
            if (numericComponent == null)
            {
                buff.BuffState = BuffState.Finished;
                return;
            }

            buff.RunTime += deltaTime;

            if (!buff.IsTrigger && buff.RunTime >= buff.BuffConfig.BuffDelayTime)
            {
                buff.IsTrigger = true;

                // 添加属性
                if (buff.BuffConfig.BuffType == 1)
                {
                    int type = buff.BuffConfig.BuffParameterType;
                    long value = buff.BuffConfig.BuffParameterValue;

                    if (value != 0)
                    {
                        if (type == NumericType.Now_Hp)
                        {
                            numericComponent.ApplyChange(type, value, true, false, buff.TheUnitFrom.Id, buff.InitBuffData.SkillConfigId, DamageType.Recover);
                        }
                        else
                        {
                            numericComponent.ApplyChange(type, value, true, false, buff.TheUnitFrom.Id, buff.InitBuffData.SkillConfigId, DamageType.Normal);
                        }
                    }
                }
            }
        }

        public override void OnFinished(BuffS buff)
        {
            if (!buff.IsTrigger)
            {
                return;
            }

            // 移除属性
            if (buff.BuffConfig.BuffType == 1)
            {
                NumericComponentS numericComponent = buff.TheUnitBelongTo.GetComponent<NumericComponentS>();

                int type = buff.BuffConfig.BuffParameterType;
                long value = buff.BuffConfig.BuffParameterValue;

                if (value != 0)
                {
                    numericComponent.ApplyChange(type, value * -1);
                }
            }
        }
    }
}