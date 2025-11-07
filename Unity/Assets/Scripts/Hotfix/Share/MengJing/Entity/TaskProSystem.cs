namespace ET
{
    [EntitySystemOf(typeof(TaskPro))]
    [FriendOf(typeof(TaskPro))]
    public static partial class TaskProSystem
    {
        [EntitySystem]
        private static void Awake(this TaskPro self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TaskPro self)
        {
        }

        public static TaskProInfo ToMessage(this TaskPro self)
        {
            TaskProInfo taskProInfo = TaskProInfo.Create();
            taskProInfo.Id = self.Id;
            taskProInfo.ConfigId = self.ConfigId;
            taskProInfo.TaskState = self.TaskState;
            taskProInfo.TaskTargetNum_1 = self.TaskTargetNum_1;
            taskProInfo.TaskTargetNum_2 = self.TaskTargetNum_2;

            return taskProInfo;
        }

        public static void FromMessage(this TaskPro self, TaskProInfo taskProInfo)
        {
            self.ConfigId = taskProInfo.ConfigId;
            self.TaskState = taskProInfo.TaskState;
            self.TaskTargetNum_1 = taskProInfo.TaskTargetNum_1;
            self.TaskTargetNum_2 = taskProInfo.TaskTargetNum_2;
        }
    }
}