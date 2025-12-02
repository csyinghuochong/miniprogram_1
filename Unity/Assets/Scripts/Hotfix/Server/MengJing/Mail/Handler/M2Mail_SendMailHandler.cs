namespace ET.Server
{
    [MessageHandler(SceneType.Mail)]
    public class M2Mail_SendMailHandler : MessageHandler<Scene, M2Mail_SendMail, Mail2M_SendMail>
    {
        protected override async ETTask Run(Scene scene, M2Mail_SendMail request, Mail2M_SendMail response)
        {
            MailCenterComponent mailCenter = scene.GetComponent<MailCenterComponent>();

            await ETTask.CompletedTask;
        }
    }
}