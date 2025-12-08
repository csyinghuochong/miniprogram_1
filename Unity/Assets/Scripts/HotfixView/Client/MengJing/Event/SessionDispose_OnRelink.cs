namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SessionDispose_OnRelink : AEvent<Scene, SessionDispose>
    {
        protected override async ETTask Run(Scene root, SessionDispose args)
        {
            int disconnectType = root.GetComponent<PlayerInfoComponent>().DisconnectType;
            root.GetComponent<PlayerInfoComponent>().DisconnectType = 0;

            switch (disconnectType)
            {
                case ErrorCode.ERR_OtherAccountLogin:
                    OnOtherAccountLogin(root);
                    break;
                case ErrorCode.ERR_SessionDisconnect:
                    OnSessionDisconnect(root);
                    break;
                case ErrorCode.ERR_KickOutPlayer:
                    //PopupTipHelp.OpenPopupTip(scene, "重新登录", "由于您长时间未操作，请重新登录！", () => { RunAsync2(scene, args, 100).Coroutine(); }).Coroutine(); 
                    break;
                case ErrorCode.ERR_PackageFrequent:
                    //PopupTipHelp.OpenPopupTip(scene, "消息异常", "请重新登录", () => { RunAsync2(scene, args, 100).Coroutine(); }).Coroutine();
                    break;
                default:
                    //EventSystem.Instance.Publish(root, new ReturnLogin());
                    OnSessionDisconnect(root);
                    break;
            }

            await ETTask.CompletedTask;
        }

        private void OnSessionDisconnect(Scene root)
        {
            root.GetComponent<RelinkComponent>().CheckRelink().Coroutine();
        }

        private void OnOtherAccountLogin(Scene root)
        {
            root.GetComponent<FloatingTextComponent>().ShowTipText("账号在其他设备登陆!");

            EventSystem.Instance.Publish(root, new ReturnLogin());
        }
    }
}