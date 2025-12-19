using Unity.Mathematics;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_NoticeUnitTransformHandler : MessageLocationHandler<Unit, C2M_NoticeUnitTransform>
    {
        protected override async ETTask Run(Unit unit, C2M_NoticeUnitTransform message)
        {
            using var _ = message;

            // 速度校验
            if (math.distance(unit.Position, message.Position) > ConfigData.PlayerSynMaxDistance)
            {
                unit.Stop();
                return;
            }

            unit.Position = message.Position;

            await ETTask.CompletedTask;
        }
    }
}