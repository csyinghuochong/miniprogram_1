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
                if (serverMail.MailReceiveType == (int)MailReceiveType.PlayerId)
                {
                    if (long.Parse(serverMail.Params) == mailUnit.Id)
                    {
                        mailComponentS.AddMail(mail.ToMessage());

                        // 领取后删除
                        serverMail.Dispose();
                        mailCenterComponent.ServerMails.RemoveAt(i);
                    }
                }
                else if (serverMail.MailReceiveType == (int)MailReceiveType.All)
                {
                    mailComponentS.AddMail(mail.ToMessage());

                    serverMail.ReceivedPlayerIds.Add(mailUnit.Id);
                }
                else if (serverMail.MailReceiveType == (int)MailReceiveType.LessLv)
                {
                    UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(scene, mailUnit.Id);
                    NumericComponentS numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponentS>(scene, mailUnit.Id);
                    InventoryComponentS inventoryComponent = await UnitCacheHelper.GetComponentCache<InventoryComponentS>(scene, mailUnit.Id);

                    // ...
                }
            }
            
            mailComponentS.Check();

            await mailUnit.AddLocation(LocationType.Mail);

            await ETTask.CompletedTask;
        }
    }
}