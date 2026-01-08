namespace ET.Server
{
    public class Buff_Damage : BuffHandler
    {
        public override void OnInit(Buff buff)
        {
        }

        public override void OnUpdate(Buff buff, float deltaTime)
        {
            buff.RunTime += deltaTime;

            if (buff.RunTime >= buff.BuffEndTime)
            {
                buff.BuffState = BuffState.Finished;
                return;
            }

            if (buff.TheUnitBelongTo == null || buff.TheUnitBelongTo.IsDisposed)
            {
                buff.BuffState = BuffState.Finished;
                return;
            }

            if (buff.TheUnitFrom == null || buff.TheUnitFrom.IsDisposed)
            {
                buff.BuffState = BuffState.Finished;
                return;
            }

            if (buff.RunTime >= buff.BuffConfig.BuffDelayTime)
            {
                // 周期触发
                if (buff.BuffConfig.BuffLoopTime > 0)
                {
                    float timeSinceDelay = buff.RunTime - buff.BuffConfig.BuffDelayTime;
                    int expectedTriggerCount = (int)(timeSinceDelay / buff.BuffConfig.BuffLoopTime);

                    if (expectedTriggerCount > buff.TriggerCount)
                    {
                        buff.TriggerCount = expectedTriggerCount;

                        Function_Fight.Fight(buff.TheUnitFrom, buff.TheUnitBelongTo, buff);
                    }
                }
                else
                {
                    // 只触发一次
                    if (!buff.IsTrigger)
                    {
                        buff.IsTrigger = true;

                        Function_Fight.Fight(buff.TheUnitFrom, buff.TheUnitBelongTo, buff);
                    }
                }
            }
        }

        public override void OnFinished(Buff buff)
        {
        }
    }
}