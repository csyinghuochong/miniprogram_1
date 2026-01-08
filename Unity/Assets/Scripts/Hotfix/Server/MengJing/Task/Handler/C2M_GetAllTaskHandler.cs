namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(TaskComponent))]
    public class C2M_GetAllTaskHandler : MessageLocationHandler<Unit, C2M_GetAllTask, M2C_GetAllTask>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllTask request, M2C_GetAllTask response)
        {
            TaskComponent taskComponent = unit.GetComponent<TaskComponent>();

            foreach (TaskPro taskPro in taskComponent.TaskProList)
            {
                response.TaskProInfoList.Add(taskPro.ToMessage());
            }
            response.CompleteTaskList.AddRange(taskComponent.CompleteTaskList);

            await ETTask.CompletedTask;
        }
    }
}