namespace ET.Client
{
    public class Buff_Base : BuffCHandler
    {
        public override void OnInit(BuffC buff)
        {
        }

        public override void OnReset(BuffC buff, float endTime)
        {
            buff.RunTime = 0f;
            buff.BuffEndTime = endTime;

            EventSystem.Instance.Publish(buff.Root(), new SkillEffectReset()
            {
                Unit = buff.TheUnitBelongTo,
                EffectInstanceId = buff.EffectInstanceId
            });
        }

        public override void OnExecute(BuffC buff)
        {
            buff.EffectInstanceId = buff.PlayBuffEffects();
            buff.BuffState = BuffState.Running;
        }

        public override void OnUpdate(BuffC buff, float deltaTime)
        {
            buff.RunTime += deltaTime;

            if (buff.RunTime >= buff.BuffEndTime)
            {
                buff.BuffState = BuffState.Finished;
                return;
            }
        }

        public override void OnFinished(BuffC buff)
        {
            EventSystem.Instance.Publish(buff.Root(), new SkillEffectFinish()
            {
                EffectInstanceId = buff.EffectInstanceId,
                Unit = buff.TheUnitBelongTo
            });
        }
    }
}