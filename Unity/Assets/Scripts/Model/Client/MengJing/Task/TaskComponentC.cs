using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class TaskComponentC : Entity, IAwake, IDestroy
    {
        public List<EntityRef<TaskPro>> TaskProList = new();

        public List<int> CompleteTaskList { get; set; } = new();
    }
}