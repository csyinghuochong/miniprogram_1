namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_EndGameLevelHandler : MessageLocationHandler<Unit, C2M_EndGameLevel, M2C_EndGameLevel>
    {
        protected override async ETTask Run(Unit unit, C2M_EndGameLevel request, M2C_EndGameLevel response)
        {
            //检测关卡信息是否正常
            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();

            int levelId = numericComponent.GetAsInt(NumericType.AdventureState);
            if (levelId == 0 || !LevelConfigCategory.Instance.DataMap.ContainsKey(levelId))
            {
                response.Error = ErrorCode.ERR_AdventureLevelIdError;
                return;
            }

            if (request.BattleInfoList.Count <= 0)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            //战斗失败直接进入垂死状态
            if (request.BattleResult == 0)
            {
                numericComponent.ApplyValue(NumericType.AdventureState, 0);
                return;
            }

            if (request.BattleResult != 1)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            //检测战斗胜利结果是否正常
            // if (!unit.GetComponent<AdventureCheckComponent>().CheckBattleWinResult(request.Round))
            // {
            //     response.Error = ErrorCode.ERR_AdventureWinResultError;
            //     return;
            // }

            numericComponent.ApplyValue(NumericType.AdventureState, 0);

            //战斗胜利增加经验值

            await ETTask.CompletedTask;
        }
    }
}