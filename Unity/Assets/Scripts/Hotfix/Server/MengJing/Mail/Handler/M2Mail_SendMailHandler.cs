using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Mail)]
    [FriendOf(typeof(ServerMail))]
    [FriendOf(typeof(MailCenterComponent))]
    public class M2Mail_SendMailHandler : MessageHandler<Scene, M2Mail_SendMail, Mail2M_SendMail>
    {
        protected override async ETTask Run(Scene scene, M2Mail_SendMail request, Mail2M_SendMail response)
        {
            try
            {
                MailCenterComponent mailCenterComponent = scene.GetComponent<MailCenterComponent>();
                MailUnitComponent mailUnitComponent = scene.GetComponent<MailUnitComponent>();

                string[] commands = request.Msg.Split('#');
                int type = int.Parse(commands[1]);
                string receivePar = commands[2];
                long time = long.Parse(commands[3]);
                string title = commands[4];
                string content = commands[5];
                string rewards = commands[6];

                MailInfo mailInfo = MailInfo.Create();
                mailInfo.Id = IdGenerater.Instance.GenerateId();
                mailInfo.Title = title;
                mailInfo.Content = content;
                mailInfo.Time = TimeHelper.ServerNow();
                mailInfo.EndTime = TimeHelper.ServerNow() + time;
                mailInfo.MailRewardComponentInfo = MailRewardComponentInfo.Create();
                foreach (string reward in rewards.Split('@'))
                {
                    string[] rewardInfo = reward.Split(';');
                    int itemId = int.Parse(rewardInfo[0]);
                    int itemNum = int.Parse(rewardInfo[1]);
                    ItemInfo itemInfo = ItemInfo.Create();
                    itemInfo.Id = IdGenerater.Instance.GenerateId();
                    itemInfo.ConfigId = itemId;
                    itemInfo.Num = itemNum;
                    mailInfo.MailRewardComponentInfo.ItemInfoList.Add(itemInfo);
                }

                mailInfo.MailRewardState = mailInfo.MailRewardComponentInfo.ItemInfoList.Count > 0 ? (int)MailRewardState.NotReceived
                        : (int)MailRewardState.NotReward;

                if (type == (int)MailReceiveType.PlayerId)
                {
                    // 发给指定玩家的
                    long unitId = long.Parse(receivePar);
                    mailUnitComponent.Children.TryGetValue(unitId, out Entity mailUnitEntity);
                    MailUnit targetMailUnit = mailUnitEntity as MailUnit;

                    if (targetMailUnit != null)
                    {
                        // 在线，直接领取邮件
                        MailComponentS mailComponentS = targetMailUnit.GetComponent<MailComponentS>();
                        if (mailComponentS != null)
                        {
                            mailComponentS.AddMail(mailInfo);
                            Mail2C_ReceiveMail message = Mail2C_ReceiveMail.Create();
                            message.MailInfo = mailInfo;
                            MapMessageHelper.SendToClient(scene, targetMailUnit.Id, message);
                        }
                    }
                    else
                    {
                        // 不在线，先保存在邮件中心，待玩家上线时再领取邮件
                        ServerMail serverMail = mailCenterComponent.AddChild<ServerMail>();
                        serverMail.MailReceiveType = (int)MailReceiveType.PlayerId;
                        serverMail.Params = receivePar;
                        Mail mail = serverMail.AddChildWithId<Mail>(mailInfo.Id);
                        mail.FromMessage(mailInfo);
                        serverMail.Mail = mail;

                        mailCenterComponent.ServerMails.Add(serverMail);
                    }
                }
                else if (type == (int)MailReceiveType.All)
                {
                    // 发给全服玩家的
                    ServerMail serverMail = mailCenterComponent.AddChild<ServerMail>();
                    serverMail.MailReceiveType = (int)MailReceiveType.All;
                    Mail mail = serverMail.AddChildWithId<Mail>(mailInfo.Id);
                    mail.FromMessage(mailInfo);
                    serverMail.Mail = mail;

                    // 在线，直接领取邮件
                    foreach (Entity entity in mailUnitComponent.Children.Values)
                    {
                        MailUnit mailUnit = entity as MailUnit;

                        if (mailUnit == null)
                        {
                            continue;
                        }

                        mailUnit.GetComponent<MailComponentS>().AddMail(mail.ToMessage());
                        Mail2C_ReceiveMail message = Mail2C_ReceiveMail.Create();
                        message.MailInfo = mailInfo;
                        MapMessageHelper.SendToClient(scene, mailUnit.Id, message);

                        serverMail.ReceivedPlayerIds.Add(mailUnit.Id);
                    }
                }
                else if (type == (int)MailReceiveType.LessLv)
                {
                    // 发给等级小于某个等级的玩家的
                    int lv = int.Parse(receivePar);
                    ServerMail serverMail = mailCenterComponent.AddChild<ServerMail>();
                    serverMail.MailReceiveType = (int)MailReceiveType.LessLv;
                    serverMail.Params = receivePar;
                    Mail mail = serverMail.AddChildWithId<Mail>(mailInfo.Id);
                    mail.FromMessage(mailInfo);
                    serverMail.Mail = mail;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
            }

            await ETTask.CompletedTask;
        }
    }
}