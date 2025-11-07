namespace ET.Client
{
    [EntitySystemOf(typeof(TaskComponentC))]
    [FriendOf(typeof(TaskComponentC))]
    public static partial class TaskComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this TaskComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TaskComponentC self)
        {
        }

        public static void OnRecvTaskUpdate(this TaskComponentC self, M2C_TaskUpdate message)
        {
            if (message.UpdateMode == 2)
            {
                foreach (TaskProInfo info in message.TaskProInfoList)
                {
                    for (int i = 0; i < self.TaskProList.Count; i++)
                    {
                        TaskPro taskPro = self.TaskProList[i];
                        if (info.ConfigId != taskPro.ConfigId)
                        {
                            continue;
                        }

                        taskPro.FromMessage(info);
                    }
                }
            }
            else
            {
                foreach (TaskPro taskPro in self.TaskProList)
                {
                    taskPro?.Dispose();
                }

                self.TaskProList.Clear();

                foreach (TaskProInfo info in message.TaskProInfoList)
                {
                    TaskPro taskPro = self.AddChildWithId<TaskPro>(info.Id);
                    taskPro.FromMessage(info);
                    self.TaskProList.Add(taskPro);
                }
            }

            self.CompleteTaskList = message.CompleteTaskList;

            EventSystem.Instance.Publish(self.Root(), new TaskUpdate());
        }

        public static TaskPro GetMainTask(this TaskComponentC self)
        {
            foreach (TaskPro taskPro in self.TaskProList)
            {
                TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskPro.ConfigId);

                if (taskConfig.TaskType == TaskType.Main)
                {
                    return taskPro;
                }
            }

            return null;
        }
        
        public static void AddTaskProFromMessage(this TaskComponentC self, TaskProInfo taskProInfo)
        {
            TaskPro taskPro = self.AddChildWithId<TaskPro>(taskProInfo.Id);
            taskPro.FromMessage(taskProInfo);
            self.TaskProList.Add(taskPro);
        }

        public static void Clear(this TaskComponentC self)
        {
            foreach (TaskPro taskPro in self.TaskProList)
            {
                taskPro?.Dispose();
            }

            self.TaskProList.Clear();
            self.CompleteTaskList.Clear();
        }
    }
}