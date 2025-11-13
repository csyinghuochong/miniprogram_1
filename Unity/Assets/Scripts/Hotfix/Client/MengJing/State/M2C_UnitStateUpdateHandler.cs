namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_UnitStateUpdateHandler : MessageHandler<Scene, M2C_UnitStateUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_UnitStateUpdate message)
        {
            Unit unit = root.CurrentScene().GetComponent<UnitComponent>().Get(message.UnitId);
            if (unit == null)
            {
                return;
            }

            // 添加状态
            if (message.StateOperateType == 1)
            {
                unit.GetComponent<StateComponentC>().StateTypeAdd((StateType)message.StateType);
            }

            //移除状态
            if (message.StateOperateType == 2)
            {
                unit.GetComponent<StateComponentC>().StateTypeRemove((StateType)message.StateType);
            }

            EventSystem.Instance.Publish(root, new StateChange() { Unit = unit, m2C_UnitStateUpdate = message });

            await ETTask.CompletedTask;
        }
    }
}