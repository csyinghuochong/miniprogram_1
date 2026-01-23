namespace ET.Server.Handler
{
    [FriendOf(typeof(BattlePassComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GetAllBattlePassHandler : MessageLocationHandler<Unit, C2M_GetAllBattlePass, M2C_GetAllBattlePass>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllBattlePass request, M2C_GetAllBattlePass response)
        {
            BattlePassComponent battlePassComponent = unit.GetComponent<BattlePassComponent>();

            foreach (BattlePass battlePass in battlePassComponent.BattlePassList)
            {
                response.BattlePassInfoList.Add(battlePass.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}