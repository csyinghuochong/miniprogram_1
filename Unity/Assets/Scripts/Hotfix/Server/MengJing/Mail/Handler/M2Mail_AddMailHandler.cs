namespace ET.Server
{
    [MessageHandler(SceneType.Mail)]
    public class M2Mail_AddMailHandler : MessageHandler<MailUnit, M2Mail_AddMail, Mail2M_AddMail>
    {
        protected override async ETTask Run(MailUnit mailUnit, M2Mail_AddMail request, Mail2M_AddMail response)
        {
            MailComponentS mailComponentS = mailUnit.GetComponent<MailComponentS>();

            mailComponentS.AddMail(request.MailInfo);

            await ETTask.CompletedTask;
        }
    }
}