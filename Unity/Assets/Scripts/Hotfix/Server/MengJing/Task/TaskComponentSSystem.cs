namespace ET.Server
{
    [EntitySystemOf(typeof(TaskComponentS))]
    [FriendOf(typeof(TaskComponentS))]
    public static partial class TaskComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this TaskComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TaskComponentS self)
        {
            self.TaskPros.Clear();
            self.CompleteTasks.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this TaskComponentS self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is TaskPro taskPro)
                {
                    self.TaskPros.Add(taskPro);
                }
            }
        }

        public static bool IsHaveTask(this TaskComponentS self, int taskConfigId)
        {
            if (self.CompleteTasks.Contains(taskConfigId))
            {
                return true;
            }

            for (int i = 0; i < self.TaskPros.Count; i++)
            {
                TaskPro taskPro = self.TaskPros[i];
                if (taskPro.ConfigId == taskConfigId)
                {
                    return true;
                }
            }

            return false;
        }

        // public static TaskPro CreateTask(this TaskComponentS self, int taskConfigId)
        // {
        //     
        // }
    }
}