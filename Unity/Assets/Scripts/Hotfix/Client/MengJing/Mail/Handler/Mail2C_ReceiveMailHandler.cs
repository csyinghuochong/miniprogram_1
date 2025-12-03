namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Mail2C_ReceiveMailHandler : MessageHandler<Scene, Mail2C_ReceiveMail>
    {
        protected override async ETTask Run(Scene root, Mail2C_ReceiveMail message)
        {
            MailComponentC mailComponentC = root.GetComponent<MailComponentC>();

            mailComponentC.AddMailFromMessage(message.MailInfo);

            await ETTask.CompletedTask;
        }
    }
}