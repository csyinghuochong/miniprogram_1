namespace ET
{
    public enum TaskState
    {
        UnActive = 0,
        Accepted = 1, //已接取
        Completed = 2, //已完成
        Commited = 3 //已领取
    }

    [ChildOf]
    public class TaskPro : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int TaskState { get; set; }
        public int TaskTargetNum_1 { get; set; }
        public int TaskTargetNum_2 { get; set; }
    }
}