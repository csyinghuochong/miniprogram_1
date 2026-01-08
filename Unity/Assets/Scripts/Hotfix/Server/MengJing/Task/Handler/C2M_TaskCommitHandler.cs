namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(TaskComponent))]
    public class C2M_TaskCommitHandler : MessageLocationHandler<Unit, C2M_TaskCommit, M2C_TaskCommit>
    {
        protected override async ETTask Run(Unit unit, C2M_TaskCommit request, M2C_TaskCommit response)
        {
            TaskComponent taskComponent = unit.GetComponent<TaskComponent>();

            response.Error = taskComponent.OnCommitTask(request.TaskConfigId);
            foreach (TaskPro taskPro in taskComponent.TaskProList)
            {
                response.TaskProInfoList.Add(taskPro.ToMessage());
            }
            response.CompleteTaskList.AddRange(taskComponent.CompleteTaskList);

            await ETTask.CompletedTask;
        }
    }
}