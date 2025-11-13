namespace ET.Server
{
    [Event(SceneType.Map)]
    public class StateTypeAdd_OnNotify : AEvent<Scene, StateTypeAdd>
    {
        protected override async ETTask Run(Scene scene, StateTypeAdd args)
        {
            Unit unit = args.UnitDefend;

            StateComponentS stateComponent = unit.GetComponent<StateComponentS>();

            M2C_UnitStateUpdate M2C_UnitStateUpdate = M2C_UnitStateUpdate.Create();
            if (stateComponent.IsStateBroadcastType(args.nowStateType))
            {
                M2C_UnitStateUpdate.UnitId = unit.Id;
                M2C_UnitStateUpdate.StateType = (long)args.nowStateType;
                M2C_UnitStateUpdate.StateValue = args.stateValue;
                M2C_UnitStateUpdate.StateOperateType = 1;
                M2C_UnitStateUpdate.StateTime = 0;
                MapMessageHelper.Broadcast(unit, M2C_UnitStateUpdate);
            }
            else
            {
                if (unit.Type == UnitType.Player)
                {
                    M2C_UnitStateUpdate.UnitId = unit.Id;
                    M2C_UnitStateUpdate.StateType = (long)args.nowStateType;
                    M2C_UnitStateUpdate.StateOperateType = 1;
                    M2C_UnitStateUpdate.StateTime = 0;

                    MapMessageHelper.SendToClient(unit, M2C_UnitStateUpdate);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}