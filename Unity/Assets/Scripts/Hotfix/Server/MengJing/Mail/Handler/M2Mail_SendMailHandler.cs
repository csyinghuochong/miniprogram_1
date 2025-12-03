namespace ET.Server
{
    [MessageHandler(SceneType.Mail)]
    public class M2Mail_SendMailHandler : MessageHandler<Scene, M2Mail_SendMail, Mail2M_SendMail>
    {
        protected override async ETTask Run(Scene scene, M2Mail_SendMail request, Mail2M_SendMail response)
        {
            MailCenterComponent mailCenterComponent = scene.GetComponent<MailCenterComponent>();
            MailUnitComponent mailUnitComponent = scene.GetComponent<MailUnitComponent>();

            if (request.UnitId == 0)
            {
                // 全局邮件
            }
            else
            {
                mailUnitComponent.Children.TryGetValue(request.UnitId, out Entity mailUnitEntity);
                MailUnit targetMailUnit = mailUnitEntity as MailUnit;

                if (targetMailUnit != null)
                {
                    MailComponentS mailComponentS = targetMailUnit.GetComponent<MailComponentS>();
                    if (mailComponentS != null)
                    {
                        mailComponentS.AddMail(request.MailInfo);
                    }
                }
                else
                {
                    MailComponentS mailComponentS = await UnitCacheHelper.GetComponent<MailComponentS>(scene, request.UnitId);

                    if (mailComponentS == null)
                    {
                        return;
                    }

                    Mail mail = mailComponentS.AddChild<Mail>();
                    mail.FromMessage(request.MailInfo);

                    await mailComponentS.SaveToDatabase();
                    mailComponentS?.Dispose();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}