using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(RelinkComponent))]
    [FriendOf(typeof(RelinkComponent))]
    public static partial class RelinkComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RelinkComponent self)
        {
            self.Relink = false;

            GameObject.Find("Global").GetComponent<Init>().OnApplicationFocusHandler = self.OnApplicationFocusHandler;
        }

        [EntitySystem]
        private static void Destroy(this RelinkComponent self)
        {
            self.Relink = false;

            GameObject.Find("Global").GetComponent<Init>().OnApplicationFocusHandler = null;
        }

        private static async ETTask CheckSession(this RelinkComponent self)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(200);

            if (self.Relink)
            {
                return;
            }

            MapComponent mapComponent = self.Root().GetComponent<MapComponent>();
            if (mapComponent.MapType < MapType.Login)
            {
                Log.Warning($"{mapComponent.MapType} 不检测");
                return;
            }

            ClientSenderComponent clientSenderComponent = self.Root().GetComponent<ClientSenderComponent>();
            NetClient2Main_CheckSession response = await clientSenderComponent.RequestCheckSession((int)mapComponent.MapType);

            Log.Warning($"NetClient2Main_CheckSession: {response.Error}");

            if (response.Error == ErrorCode.ERR_Success)
            {
                return;
            }

            self.CheckRelink().Coroutine();
        }

        private static void OnApplicationFocusHandler(this RelinkComponent self, bool value)
        {
            if (value)
            {
                // Log.Warning("获得焦点！！");
                self.CheckSession().Coroutine();
            }
        }

        public static async ETTask CheckRelink(this RelinkComponent self)
        {
            if (self.Relink)
            {
                return;
            }

            self.Relink = true;

            self.Root().GetComponent<UIComponent>().Create(UIType.UIRelink).Coroutine();

            TimerComponent timerComponent = self.Root().GetComponent<TimerComponent>();
            for (int i = 0; i < 5; i++)
            {
                long instanceId = self.InstanceId;

                Log.Warning($"重连请求  {i} ！！ {self.Relink}");
                if (timerComponent == null || !self.Relink)
                {
                    break;
                }

                await timerComponent.WaitAsync(1000);

                if (instanceId != self.InstanceId)
                {
                    break;
                }

                if (timerComponent == null || !self.Relink)
                {
                    break;
                }

                await self.SendLogin();

                if (i == 4)
                {
                    EventSystem.Instance.Publish(self.Root(), new ReturnLogin());
                    break;
                }
            }

            self.Root().GetComponent<UIComponent>().Remove(UIType.UIRelink);
        }

        // 断线重连，重新走登录流程
        private static async ETTask<int> SendLogin(this RelinkComponent self)
        {
            Scene root = self.Root();
            int errorCode = ErrorCode.ERR_Success;
            PlayerInfoComponent playerInfoComponent = root.GetComponent<PlayerInfoComponent>();
            errorCode = await LoginHelper.Login(root, playerInfoComponent.Account, playerInfoComponent.Password, 1, playerInfoComponent.VersionMode);
            if (errorCode != ErrorCode.ERR_Success)
            {
                return errorCode;
            }

            errorCode = await LoginHelper.LoginGameAsync(root, 1);
            return errorCode;
        }
    }
}