namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_RechallengeHandler : MessageLocationHandler<Unit, C2M_Rechallenge, M2C_Rechallenge>
    {
        protected override async ETTask Run(Unit unit, C2M_Rechallenge request, M2C_Rechallenge response)
        {
            MapComponent mapComponent = unit.Scene().GetComponent<MapComponent>();

            if (mapComponent.MapType != MapType.LocalLevel)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            unit.Scene().GetComponent<LocalLevelComponent>().ResetLevel();

            await ETTask.CompletedTask;
        }
    }
}