using Unity.Mathematics;

namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_NoticeUnitTransformListHandler : MessageHandler<Scene, M2C_NoticeUnitTransformList>
    {
        protected override async ETTask Run(Scene root, M2C_NoticeUnitTransformList message)
        {
            using var _ = message;

            Scene currentScene = root.CurrentScene();
            if (currentScene == null)
            {
                return;
            }

            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();

            if (unitComponent == null)
            {
                return;
            }

            for (int i = 0; i < message.UnitIdList.Count; i++)
            {
                Unit unit = unitComponent.Get(message.UnitIdList[i]);
                if (unit == null)
                {
                    continue;
                }

                float3 position = message.PositionList[i];

                if (unit.MainHero)
                {
                    if (math.distance(unit.Position, position) > ConfigData.PlayerSynMaxDistance)
                    {
                        unit.Position = position;
                    }

                    continue;
                }

                unit.Position = position;
            }

            await ETTask.CompletedTask;
        }
    }
}