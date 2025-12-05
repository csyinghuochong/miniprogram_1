using System.Collections.Generic;

namespace ET.Server.Handler
{
    [MessageHandler(SceneType.Mail)]
    [FriendOf(typeof(MailCenterComponent))]
    [FriendOf(typeof(ServerMail))]
    public class G2Mail_LoginMailServerHandler : MessageHandler<Scene, G2Mail_LoginMailServer, Mail2G_LoginMailServer>
    {
        protected override async ETTask Run(Scene scene, G2Mail_LoginMailServer request, Mail2G_LoginMailServer response)
        {
            MailCenterComponent mailCenterComponent = scene.GetComponent<MailCenterComponent>();
            MailUnitComponent mailUnitComponent = scene.GetComponent<MailUnitComponent>();
            mailUnitComponent.Children.TryGetValue(request.UnitId, out Entity mailUnitEntity);

            MailUnit mailUnit = mailUnitEntity as MailUnit;

            if (mailUnit != null)
            {
                return;
            }

            mailUnit = mailUnitComponent.AddChildWithId<MailUnit>(request.UnitId);
            mailUnit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);

            MailComponentS mailComponentS = await UnitCacheHelper.GetComponent<MailComponentS>(scene, request.UnitId);

            if (mailComponentS == null)
            {
                mailComponentS = mailUnit.AddComponent<MailComponentS>();
            }
            else
            {
                mailUnit.AddComponent(mailComponentS);
            }

            // 从邮件中心服领取邮件
            for (int i = mailCenterComponent.ServerMails.Count - 1; i >= 0; i--)
            {
                ServerMail serverMail = mailCenterComponent.ServerMails[i];

                if (serverMail.ReceivedPlayerIds.Contains(mailUnit.Id))
                {
                    continue;
                }

                Mail mail = serverMail.Mail;
                bool shouldReceive = false;
                bool shouldRemove = false;

                switch ((MailReceiveType)serverMail.MailReceiveType)
                {
                    case MailReceiveType.PlayerId:
                    {
                        if (long.TryParse(serverMail.Params, out long targetId) && targetId == mailUnit.Id)
                        {
                            shouldReceive = true;
                            shouldRemove = true;
                        }

                        break;
                    }
                    case MailReceiveType.All:
                    {
                        shouldReceive = true;
                        break;
                    }
                    case MailReceiveType.LessLv:
                    {
                        UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(scene, mailUnit.Id);
                        if (int.TryParse(serverMail.Params, out int targetLevel) && userInfoComponent.GetLv() < targetLevel)
                        {
                            shouldReceive = true;
                        }

                        break;
                    }
                }

                if (shouldReceive)
                {
                    mailComponentS.AddMail(mail.ToMessage());

                    if (shouldRemove)
                    {
                        mailCenterComponent.RemoveServerMailAt(i);
                    }
                    else
                    {
                        serverMail.ReceivedPlayerIds.Add(mailUnit.Id);
                    }
                }
            }

            mailComponentS.Check();

            await mailUnit.AddLocation(LocationType.Mail);

            await ETTask.CompletedTask;
        }
    }
}