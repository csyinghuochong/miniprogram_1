namespace ET.Server
{
    public class AIHandlerAttribute : BaseAttribute
    {
    }

    [AIHandler]
    [EnableClass]
    public abstract class AAIHandler
    {
        // 检查是否满足条件
        // 返回0表示满足该AI行为条件，不再继续检查后续 AI 配置。返回非0是继续遍历下一个AI配置
        public abstract int Check(AIComponent aiComponent, AIConfig aiConfig);

        // 协程编写必须可以取消
        public abstract ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken);
    }
}