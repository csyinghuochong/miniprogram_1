namespace ET.Client
{
    public class BuffHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [BuffHandler]
    public abstract class BuffHandler
    {
        /// <summary>
        /// 初始化buff数据
        /// </summary>
        /// <param name="buff">Buff数据</param>
        /// <param name="theUnitFrom">来自哪个Unit</param>
        /// <param name="theUnitBelongTo">寄生于哪个Unit</param>
        /// <param name="skill"></param>
        public abstract void OnInit(Buff buff, Unit theUnitFrom, Unit theUnitBelongTo, Skill skill);

        /// <summary>
        /// Buff持续
        /// </summary>
        public abstract void OnUpdate(Buff buff);

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public abstract void OnFinished(Buff buff);
    }
}