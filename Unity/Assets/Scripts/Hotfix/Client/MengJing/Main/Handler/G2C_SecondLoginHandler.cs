namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class G2C_SecondLoginHandler: MessageHandler<Scene, G2C_SecondLogin>
    {
        protected override async ETTask Run(Scene root, G2C_SecondLogin message)
        {
            Log.Debug("G2C_SecondLoginHandler..重连成功！！");
            //也可以放在 LoginHelper.LoginGameAsync
            
            // await UserInfoNetHelper.RequestUserInfoInit(root);
            // await UserInfoNetHelper.RequestFreshUnit(root);
            EventSystem.Instance.Publish(root, new RelinkSucess());

            await ETTask.CompletedTask;
        }
    }
}