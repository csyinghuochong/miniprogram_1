namespace ET.Server
{
    public class BuffSHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [BuffSHandler]
    public abstract class BuffSHandler
    {
        /// <summary>
        /// 初始化buff数据
        /// </summary>
        /// <param name="buff">Buff数据</param>
        /// <param name="theUnitFrom">来自哪个Unit</param>
        /// <param name="theUnitBelongTo">寄生于哪个Unit</param>
        /// <param name="skill"></param>
        public abstract void OnInit(BuffS buff, Unit theUnitFrom, Unit theUnitBelongTo, SkillS skill);

        /// <summary>
        /// Buff持续
        /// </summary>
        public abstract void OnUpdate(BuffS buff);

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public abstract void OnFinished(BuffS buff);
    }
}