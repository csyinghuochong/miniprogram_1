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
        public abstract void OnInit(BuffS buff);

        /// <summary>
        /// Buff持续
        /// </summary>
        public abstract void OnUpdate(BuffS buff, float deltaTime);

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public abstract void OnFinished(BuffS buff);
    }
}