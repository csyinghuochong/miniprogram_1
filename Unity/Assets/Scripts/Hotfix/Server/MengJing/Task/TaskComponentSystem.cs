using System.Collections.Generic;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class TriggerTask_Notify : AEvent<Scene, TriggerTask>
    {
        protected override async ETTask Run(Scene scene, TriggerTask args)
        {
            args.Unit.GetComponent<TaskComponent>()?.TriggerTaskEvent(args.TargetType, args.TargetId, args.TargetValue);

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(TaskComponent))]
    [FriendOf(typeof(TaskComponent))]
    public static partial class TaskComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TaskComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TaskComponent self)
        {
            self.TaskProList.Clear();
            self.CompleteTaskList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this TaskComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is TaskPro taskPro)
                {
                    self.TaskProList.Add(taskPro);
                }
            }
        }

        public static void OnLogin(this TaskComponent self)
        {
            // 领取主线任务
            int mainTaskId = 10010000;
            if (!self.IsHaveTask(mainTaskId))
            {
                TaskConfig taskConfig = TaskConfigCategory.Instance.Get(mainTaskId);
                TaskPro TaskPro = self.CreateTask(mainTaskId);
            }

            // 重新触发一些任务
            UserInfoComponent userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponent>();
            self.TriggerTaskEvent(TaskTargetType.PlayerLv, 0, userInfoComponent.GetLv(), false);
        }

        public static bool IsHaveTask(this TaskComponent self, int taskConfigId)
        {
            if (self.CompleteTaskList.Contains(taskConfigId))
            {
                return true;
            }

            for (int i = 0; i < self.TaskProList.Count; i++)
            {
                TaskPro taskPro = self.TaskProList[i];
                if (taskPro.ConfigId == taskConfigId)
                {
                    return true;
                }
            }

            return false;
        }

        public static TaskPro CreateTask(this TaskComponent self, int taskConfigId)
        {
            Unit unit = self.GetParent<Unit>();
            TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskConfigId);

            TaskPro taskPro = self.AddChild<TaskPro>();
            taskPro.ConfigId = taskConfigId;

            self.TaskProList.Add(taskPro);

            switch (taskConfig.TargetType)
            {
                case TaskTargetType.PlayerLv:
                {
                    taskPro.TaskTargetNum_1 = unit.GetComponent<UserInfoComponent>().GetLv();
                    break;
                }
                case TaskTargetType.KillMonster:
                {
                    break;
                }
                case TaskTargetType.KillBOSS:
                {
                    break;
                }
                case TaskTargetType.KillMonsterId:
                {
                    break;
                }
                case TaskTargetType.PassLeveld:
                {
                    break;
                }
                case TaskTargetType.CombatPower:
                {
                    break;
                }
            }

            bool completed = self.IsCompleted(taskPro);
            taskPro.TaskState = completed ? (int)TaskState.Completed : (int)TaskState.Accepted;

            return taskPro;
        }

        private static bool IsCompleted(this TaskComponent self, TaskPro taskPro)
        {
            TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskPro.ConfigId);

            for (int i = 0; i < taskConfig.TargetId.Length; i++)
            {
                if (i == 0 && taskConfig.TargetValue[i] > taskPro.TaskTargetNum_1)
                {
                    return false;
                }

                if (i == 1 && taskConfig.TargetValue[i] > taskPro.TaskTargetNum_2)
                {
                    return false;
                }
            }

            return true;
        }

        public static void TriggerTaskEvent(this TaskComponent self, TaskTargetType targetType, int targetId, int targetValue, bool notice = true)
        {
            List<TaskPro> noticeTaskList = new();

            for (int i = 0; i < self.TaskProList.Count; i++)
            {
                TaskPro taskPro = self.TaskProList[i];

                TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskPro.ConfigId);

                if (taskConfig.TargetType != targetType)
                {
                    continue;
                }

                if (taskPro.TaskState >= (int)TaskState.Completed)
                {
                    continue;
                }

                switch (taskConfig.TargetType)
                {
                    case TaskTargetType.PlayerLv:
                    {
                        taskPro.TaskTargetNum_1 = targetValue;
                        break;
                    }
                    case TaskTargetType.KillMonster:
                    {
                        taskPro.TaskTargetNum_1++;
                        break;
                    }
                    case TaskTargetType.KillBOSS:
                    {
                        taskPro.TaskTargetNum_1++;

                        break;
                    }
                    case TaskTargetType.KillMonsterId:
                    {
                        if (taskConfig.TargetId[0] == targetId)
                        {
                            taskPro.TaskTargetNum_1++;
                        }

                        break;
                    }
                    case TaskTargetType.PassLeveld:
                    {
                        break;
                    }
                    case TaskTargetType.CombatPower:
                    {
                        taskPro.TaskTargetNum_1 = targetValue;
                        break;
                    }
                }

                bool completed = self.IsCompleted(taskPro);
                taskPro.TaskState = completed ? (int)TaskState.Completed : (int)TaskState.Accepted;

                noticeTaskList.Add(taskPro);
            }

            if (notice && noticeTaskList.Count > 0)
            {
                self.NoticeUpdateOneTask(noticeTaskList);
            }
        }

        private static void NoticeUpdateOneTask(this TaskComponent self, List<TaskPro> taskProList)
        {
            Unit unit = self.GetParent<Unit>();
            M2C_TaskUpdate m2C_TaskUpdate = M2C_TaskUpdate.Create();
            m2C_TaskUpdate.UpdateMode = 2;
            foreach (TaskPro taskPro in taskProList)
            {
                m2C_TaskUpdate.TaskProInfoList.Add(taskPro.ToMessage());
            }

            m2C_TaskUpdate.CompleteTaskList.AddRange(self.CompleteTaskList);
            MapMessageHelper.SendToClient(unit, m2C_TaskUpdate);
        }

        private static void NoticeUpdateAllTask(this TaskComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            M2C_TaskUpdate m2C_TaskUpdate = M2C_TaskUpdate.Create();
            m2C_TaskUpdate.UpdateMode = 2;
            foreach (TaskPro taskPro in self.TaskProList)
            {
                m2C_TaskUpdate.TaskProInfoList.Add(taskPro.ToMessage());
            }

            m2C_TaskUpdate.CompleteTaskList.AddRange(self.CompleteTaskList);
            MapMessageHelper.SendToClient(unit, m2C_TaskUpdate);
        }

        public static int OnCommitTask(this TaskComponent self, int taskConfigId)
        {
            if (self.CompleteTaskList.Contains(taskConfigId))
            {
                return ErrorCode.ERR_ModifyData;
            }

            TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskConfigId);

            TaskPro taskPro = self.GetTaskByConfigId(taskConfigId);
            if (taskPro == null)
            {
                return ErrorCode.ERR_TaskCommited;
            }

            if (taskPro.TaskState != (int)TaskState.Completed)
            {
                return ErrorCode.ERR_TaskNoCompleted;
            }

            Unit unit = self.GetParent<Unit>();

            if (taskConfig.TaskType == TaskType.Main)
            {
                for (int i = self.TaskProList.Count - 1; i >= 0; i--)
                {
                    TaskPro t = self.TaskProList[i];
                    if (t.ConfigId == taskConfigId)
                    {
                        t.TaskState = (int)TaskState.Commited;
                        t.Dispose();
                        self.TaskProList.RemoveAt(i);
                    }
                }

                if (!self.CompleteTaskList.Contains(taskConfigId))
                {
                    self.CompleteTaskList.Add(taskConfigId);
                }

                // 自动接取下一个主线任务
                foreach (TaskConfig config in TaskConfigCategory.Instance.DataList)
                {
                    if (config.Id > taskConfigId && config.TaskType == TaskType.Main)
                    {
                        self.CreateTask(config.Id);
                        break;
                    }
                }
            }
            else
            {
                for (int i = self.TaskProList.Count - 1; i >= 0; i--)
                {
                    TaskPro t = self.TaskProList[i];
                    if (t.ConfigId == taskConfigId)
                    {
                        t.TaskState = (int)TaskState.Commited;
                        t.Dispose();
                        self.TaskProList.RemoveAt(i);
                    }
                }
            }

            List<RewardItem> rewardItems = new();
            rewardItems.Add(new RewardItem() { ItemId = ConfigData.Item_Gold, ItemNum = taskConfig.TaskGold });
            rewardItems.Add(new RewardItem() { ItemId = ConfigData.Item_Exp, ItemNum = taskConfig.TaskExp });
            rewardItems.AddRange(taskConfig.RewardItem);

            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();
            inventoryComponent.AddItemData(rewardItems);

            return ErrorCode.ERR_Success;
        }

        public static TaskPro GetTaskByConfigId(this TaskComponent self, int taskConfigId)
        {
            foreach (TaskPro taskPro in self.TaskProList)
            {
                if (taskPro.ConfigId == taskConfigId)
                {
                    return taskPro;
                }
            }

            return null;
        }

        public static void OnKillUnit(this TaskComponent self, Unit defendUnit, MapType mapType)
        {
            if (defendUnit.Type == UnitType.Monster)
            {
                self.TriggerTaskEvent(TaskTargetType.KillMonster, 0, 1);
                self.TriggerTaskEvent(TaskTargetType.KillMonsterId, defendUnit.ConfigId, 1);
            }
        }
    }
}