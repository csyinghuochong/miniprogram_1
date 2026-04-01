namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_PathfindingResultHandler : MessageHandler<Scene, M2C_PathfindingResult>
    {
        protected override async ETTask Run(Scene root, M2C_PathfindingResult message)
        {
            using var _ = message;
            
            Unit unit = root.CurrentScene().GetComponent<UnitComponent>().Get(message.Id);
            if (unit == null)
            {
                return;
            }

            if (!unit.MainHero)
            {
                EventSystem.Instance.Publish(root.CurrentScene(), new MoveStart() { Unit = unit });
                float speed = unit.GetComponent<NumericComponentC>().GetAsFloat(NumericType.Now_MoveSpeed);
                unit.GetComponent<Move2DComponent>().MoveTo(message.Points, speed);

                // float speed = unit.GetComponent<NumericComponentC>().GetAsFloat(NumericType.Now_MoveSpeed);
                // speed *= (message.SpeedRate * 0.01f);
                // unit.GetComponent<MoveComponent>().MoveToAsync(message.Points, speed).Coroutine();
            }

            await ETTask.CompletedTask;
        }
    }
}