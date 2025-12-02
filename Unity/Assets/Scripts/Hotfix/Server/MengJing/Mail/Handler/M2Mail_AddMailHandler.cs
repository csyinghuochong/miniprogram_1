namespace ET.Server
{
    [MessageHandler(SceneType.Mail)]
    public class M2Mail_AddMailHandler : MessageHandler<Scene, M2Mail_AddMail, Mail2M_AddMail>
    {
        protected override async ETTask Run(Scene scene, M2Mail_AddMail request, Mail2M_AddMail response)
        {
            MailCenterComponent mailCenter = scene.GetComponent<MailCenterComponent>();

            await ETTask.CompletedTask;
        }
    }
}