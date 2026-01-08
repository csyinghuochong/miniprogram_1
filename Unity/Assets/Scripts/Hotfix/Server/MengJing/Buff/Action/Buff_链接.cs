namespace ET.Server
{
    /// <summary>
    /// BuffParameterValue 伤害比例
    /// </summary>
    public class Buff_链接 : BuffHandler
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
            }
        }

        public override void OnFinished(Buff buff)
        {
        }
    }
}