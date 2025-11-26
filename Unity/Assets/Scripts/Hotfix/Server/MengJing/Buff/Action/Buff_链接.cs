namespace ET.Server
{
    /// <summary>
    /// BuffParameterValue 伤害比例
    /// </summary>
    public class Buff_链接 : BuffSHandler
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
            }
        }

        public override void OnFinished(BuffS buff)
        {
        }
    }
}