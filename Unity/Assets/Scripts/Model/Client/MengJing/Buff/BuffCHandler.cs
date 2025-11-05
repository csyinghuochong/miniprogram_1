namespace ET.Client
{
    public class BuffCHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [BuffCHandler]
    public abstract class BuffHandler
    {
        /// <summary>
        /// 初始化buff数据
        /// </summary>
        /// <param name="buffC">Buff数据</param>
        /// <param name="theUnitFrom">来自哪个Unit</param>
        /// <param name="theUnitBelongTo">寄生于哪个Unit</param>
        /// <param name="skillC"></param>
        public abstract void OnInit(BuffC buffC, Unit theUnitFrom, Unit theUnitBelongTo, SkillC skillC);

        /// <summary>
        /// Buff持续
        /// </summary>
        public abstract void OnUpdate(BuffC buffC);

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public abstract void OnFinished(BuffC buffC);
    }
}