using Unity.Mathematics;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_StopResultHandler : MessageLocationHandler<Unit, C2M_StopResult>
    {
        protected override async ETTask Run(Unit unit, C2M_StopResult message)
        {
            // float3 stopPos;
            // // 不能完全相信客户端
            // if (math.distance(unit.Position, message.Position) > 2f)
            // {
            //     stopPos = unit.Position;
            // }
            // else
            // {
            //     stopPos = message.Position;
            // }

            unit.StopResult(unit.Position, 0);

            await ETTask.CompletedTask;
        }
    }
}