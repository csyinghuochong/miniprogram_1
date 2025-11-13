namespace ET.Server
{
    public class Buff_Attribute : BuffSHandler
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

            NumericComponentS numericComponent = buff.TheUnitBelongTo.GetComponent<NumericComponentS>();
            if (numericComponent == null)
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

                        TriggerBuffEffect(buff, numericComponent);
                    }
                }
                else
                {
                    if (!buff.IsTrigger)
                    {
                        buff.IsTrigger = true;

                        TriggerBuffEffect(buff, numericComponent);
                    }
                }
            }
        }

        private void TriggerBuffEffect(BuffS buff, NumericComponentS numericComponent)
        {
            switch (buff.BuffConfig.BuffType)
            {
                // 属性
                case 1:
                {
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
                            numericComponent.ApplyChange(type, value, true, false, buff.TheUnitFrom.Id, buff.InitBuffData.SkillConfigId, DamageType.Recover);
                        }
                        else
                        {
                            numericComponent.ApplyChange(type, value, true, false, buff.TheUnitFrom.Id, buff.InitBuffData.SkillConfigId, DamageType.Normal);
                        }
                    }

                    break;
                }
                // 状态
                case 2:
                {
                    buff.TheUnitBelongTo.GetComponent<StateComponentS>().StateTypeAdd((StateType)buff.BuffConfig.BuffParameterType);
                    break;
                }
            }
        }

        public override void OnFinished(BuffS buff)
        {
            if (!buff.IsTrigger && buff.BuffConfig.BuffLoopTime <= 0)
            {
                return;
            }

            switch (buff.BuffConfig.BuffType)
            {
                // 移除属性
                case 1:
                {
                    NumericComponentS numericComponent = buff.TheUnitBelongTo.GetComponent<NumericComponentS>();

                    int type = buff.BuffConfig.BuffParameterType;
                    long value = buff.BuffConfig.BuffParameterValue;

                    if (value != 0)
                    {
                        if (type > 100000)
                        {
                            numericComponent.ApplyChange(type, value * -1);
                        }
                    }

                    break;
                }
                // 移除状态
                case 2:
                {
                    buff.TheUnitBelongTo.GetComponent<StateComponentS>().StateTypeRemove((StateType)buff.BuffConfig.BuffParameterType);
                    break;
                }
            }
        }
    }
}