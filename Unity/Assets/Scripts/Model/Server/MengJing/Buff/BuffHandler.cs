namespace ET.Server
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
        public abstract void OnInit(Buff buff);

        /// <summary>
        /// Buff持续
        /// </summary>
        public abstract void OnUpdate(Buff buff, float deltaTime);

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public abstract void OnFinished(Buff buff);
    }
}