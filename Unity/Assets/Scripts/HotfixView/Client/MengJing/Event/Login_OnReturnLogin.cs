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
            scene.Root().RemoveComponent<ClientSenderComponent>();
            oldRoot.CurrentScene()?.Dispose();
            oldRoot.GetComponent<UIComponent>().RemoveAll();
            GameObject.Find("Global").GetComponent<Init>().TogglePatchWindow(true);

            // await FiberManager.Instance.Remove(oldRoot.Fiber.Id);
            // await FiberManager.Instance.Create(SchedulerType.Main, ConstFiberId.Main, 0, SceneType.Main, "");
            await EventSystem.Instance.PublishAsync(scene, new AppStartInitFinish());

            GameObject.Find("Global").GetComponent<Init>().TogglePatchWindow(false);
        }
    }
}