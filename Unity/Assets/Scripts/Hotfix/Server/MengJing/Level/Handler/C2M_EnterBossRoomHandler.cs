namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponent))]
    public class C2M_EnterBossRoomHandler : MessageLocationHandler<Unit, C2M_EnterBossRoom, M2C_EnterBossRoom>
    {
        protected override async ETTask Run(Unit unit, C2M_EnterBossRoom request, M2C_EnterBossRoom response)
        {
            MapComponent mapComponent = unit.Scene().GetComponent<MapComponent>();

            if (mapComponent.MapType != MapType.LocalLevel)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }
            
            unit.Scene().GetComponent<LocalLevelComponent>().EnterBossRoom();
            
            await ETTask.CompletedTask;
        }
    }
}