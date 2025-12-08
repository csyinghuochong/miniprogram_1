using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class Login_OnReturnLogin : AEvent<Scene, ReturnLogin>
    {
        protected override async ETTask Run(Scene scene, ReturnLogin args)
        {
            MapComponent mapComponent = scene.GetComponent<MapComponent>();
            Log.Warning($"ReturnLogin.mapComponent.MapType  {mapComponent.MapType}");
            if (mapComponent.MapType == MapType.Login)
            {
                return;
            }

            mapComponent.MapType = MapType.Login;
            RunAsync2(scene, args, 100).Coroutine();

            await ETTask.CompletedTask;
        }

        private async ETTask RunAsync2(Scene scene, ReturnLogin args, long waitTime)
        {
            long instanceId = scene.InstanceId;
            TimerComponent timerComponent = scene.GetComponent<TimerComponent>();
            await timerComponent.WaitAsync(waitTime);
            if (instanceId != scene.InstanceId)
            {
                return;
            }

            Scene oldRoot = scene.Root();
            Log.Warning($"ReturnLogin.RunAsync2  {oldRoot.Fiber.Id}");
            scene.Root().RemoveComponent<ClientSenderComponent>();
            Log.Warning($"ReturnLogin  {oldRoot.CurrentScene()}   {oldRoot.CurrentScene()?.GetComponent<UnitComponent>()?.Children.Count}");
            oldRoot.CurrentScene()?.Dispose();
            oldRoot.GetComponent<UIComponent>().RemoveAll();
            GameObject.Find("Global").GetComponent<Init>().TogglePatchWindow(true);
            await FiberManager.Instance.Remove(oldRoot.Fiber.Id);
            // 这里发现个问题，当从游戏中返回登录界面从新登录游戏后。IUpdate和ILateUpdate会多调用一次
            // 定位到 Create时 this.schedulers[(int) schedulerType].Add(fiberId); 会添加重复的fiberId
            // Scheduler执行Update()时会重复调用相同的Fiber
            await FiberManager.Instance.Create(SchedulerType.Main, ConstFiberId.Main, 0, SceneType.Main, "");
            GameObject.Find("Global").GetComponent<Init>().TogglePatchWindow(false);
        }
    }
}