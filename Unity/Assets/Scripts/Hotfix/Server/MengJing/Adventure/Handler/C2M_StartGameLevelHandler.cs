namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_StartGameLevelHandler : MessageLocationHandler<Unit, C2M_StartGameLevel, M2C_StartGameLevel>
    {
        protected override async ETTask Run(Unit unit, C2M_StartGameLevel request, M2C_StartGameLevel response)
        {
            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();

            if (numericComponent.GetAsInt(NumericType.AdventureState) != 0)
            {
                response.Error = ErrorCode.ERR_AlreadyAdventureState;
                return;
            }

            if (!LevelConfigCategory.Instance.DataMap.ContainsKey(request.LevelId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            LevelConfig config = LevelConfigCategory.Instance.Get(request.LevelId);
            // if (numericComponent[NumericType.Level] < config.MiniEnterLevel[0])
            // {
            //     response.Error = ErrorCode.ERR_AdventureLevelNotEnough;
            //     return;
            // }

            numericComponent.ApplyValue(NumericType.AdventureState, request.LevelId);
            numericComponent.ApplyValue(NumericType.AdventureStartTime, TimeHelper.ServerNow());
            //设置本次战斗的随机种子，保证客户端的战斗中的每次随机产生的数能在服务器端复现
            numericComponent.ApplyValue(NumericType.BattleRandomSeed, RandomHelper.RandUInt32());

            await ETTask.CompletedTask;
        }
    }
}