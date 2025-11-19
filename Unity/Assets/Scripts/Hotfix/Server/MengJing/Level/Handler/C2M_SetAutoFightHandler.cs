using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_SetAutoFightHandler: MessageLocationHandler<Unit, C2M_SetAutoFight, M2C_SetAutoFight>
    {
        protected override async ETTask Run(Unit unit, C2M_SetAutoFight request, M2C_SetAutoFight response)
        {
            MapComponent mapComponent = unit.Scene().GetComponent<MapComponent>();

            if (mapComponent.MapType != MapType.LocalLevel)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }
            
            List<EntityRef<Unit>> allUnits = unit.GetParent<UnitComponent>().GetAll();
            foreach (Unit u in allUnits)
            {
                if (u.Type == UnitType.Hero)
                {
                    u.GetComponent<AIComponent>().AutoUseSkill = request.Value != 0;
                }
            }
            
            await ETTask.CompletedTask;
        }
    }
}