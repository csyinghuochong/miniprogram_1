namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(Scene))]
    public class C2M_SetTimeScaleHandler : MessageLocationHandler<Unit, C2M_SetTimeScale, M2C_SetTimeScale>
    {
        protected override async ETTask Run(Unit unit, C2M_SetTimeScale request, M2C_SetTimeScale response)
        {
            MapComponent mapComponent = unit.Scene().GetComponent<MapComponent>();

            // if (mapComponent.MapType == MapTypeEnum.MainCityScene)
            // {
            //     response.Error = ErrorCode.ERR_SceneCantSetTimeScale;
            //     return;
            // }

            // 0-3倍之间
            if (request.TimeScale < 0 || request.TimeScale > 3)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            unit.Scene().TimeScale = request.TimeScale;

            M2C_UpdateTimeScale m2CUpdateTimeScale = M2C_UpdateTimeScale.Create();
            m2CUpdateTimeScale.TimeScale = request.TimeScale;
            MapMessageHelper.Broadcast(unit.Scene(), m2CUpdateTimeScale);

            await ETTask.CompletedTask;
        }
    }
}