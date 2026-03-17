namespace ET.Client
{
    public static class ClientTaskHelper
    {
        public static async ETTask<int> GetAllTask(Scene root)
        {
            C2M_GetAllTask request = C2M_GetAllTask.Create();

            M2C_GetAllTask response = (M2C_GetAllTask)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            TaskComponentC heroComponentC = root.GetComponent<TaskComponentC>();
            heroComponentC.Clear();
            foreach (TaskProInfo info in response.TaskProInfoList)
            {
                heroComponentC.AddTaskProFromMessage(info);
            }

            heroComponentC.CompleteTaskList = response.CompleteTaskList;

            return response.Error;
        }

        public static async ETTask<int> TaskCommit(Scene root, int taskConfigId)
        {
            C2M_TaskCommit request = C2M_TaskCommit.Create();
            request.TaskConfigId = taskConfigId;

            M2C_TaskCommit response = (M2C_TaskCommit)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            TaskComponentC heroComponentC = root.GetComponent<TaskComponentC>();
            heroComponentC.Clear();
            foreach (TaskProInfo info in response.TaskProInfoList)
            {
                heroComponentC.AddTaskProFromMessage(info);
            }

            heroComponentC.CompleteTaskList = response.CompleteTaskList;

            EventSystem.Instance.Publish(root, new TaskCommit() { TaskConfigId = taskConfigId });
            EventSystem.Instance.Publish(root, new TaskUpdate());

            return response.Error;
        }
    }
}