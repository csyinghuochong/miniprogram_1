using System;
using System.Collections.Generic;

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

                if (commands.Length < 7)
                {
                    Log.Error($"发送邮件参数不足: {request.Msg}");
                    return;
                }

                if (!int.TryParse(commands[1], out int type))
                {
                    Log.Error($"邮件类型解析失败: {commands[1]}");
                    return;
                }

                string receivePar = commands[2];

                if (!long.TryParse(commands[3], out long time))
                {
                    Log.Error($"邮件有效期解析失败: {commands[3]}");
                    return;
                }

                string title = commands[4];
                string content = commands[5];
                string rewards = commands[6];

                MailInfo mailInfo = CreateMailInfo(title, content, time, rewards);

                switch ((MailReceiveType)type)
                {
                    case MailReceiveType.PlayerId:
                        await SendToPlayer(scene, mailCenterComponent, mailUnitComponent, mailInfo, receivePar);
                        break;
                    case MailReceiveType.All:
                        await SendToAllPlayers(scene, mailCenterComponent, mailUnitComponent, mailInfo);
                        break;
                    case MailReceiveType.LessLv:
                        await SendToLowLevelPlayers(scene, mailCenterComponent, mailUnitComponent, mailInfo, receivePar);
                        break;
                    default:
                        Log.Error($"未知的邮件类型: {type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            await ETTask.CompletedTask;
        }

        private static MailInfo CreateMailInfo(string title, string content, long expireTime, string rewards)
        {
            MailInfo mailInfo = MailInfo.Create();
            mailInfo.Id = IdGenerater.Instance.GenerateId();
            mailInfo.From = "官方";
            mailInfo.Title = title;
            mailInfo.Content = content;
            mailInfo.Time = TimeHelper.ServerNow();
            mailInfo.EndTime = TimeHelper.ServerNow() + expireTime;
            mailInfo.MailRewardComponentInfo = MailRewardComponentInfo.Create();

            if (!string.IsNullOrEmpty(rewards))
            {
                string[] rewardItems = rewards.Split('@');
                foreach (string reward in rewardItems)
                {
                    if (string.IsNullOrEmpty(reward))
                    {
                        continue;
                    }

                    string[] rewardInfo = reward.Split(';');
                    if (rewardInfo.Length < 2)
                    {
                        continue;
                    }

                    if (!int.TryParse(rewardInfo[0], out int itemId) || !int.TryParse(rewardInfo[1], out int itemNum))
                    {
                        Log.Error($"奖励解析失败: {reward}");
                        continue;
                    }

                    ItemInfo itemInfo = ItemInfo.Create();
                    itemInfo.Id = IdGenerater.Instance.GenerateId();
                    itemInfo.ConfigId = itemId;
                    itemInfo.Num = itemNum;
                    mailInfo.MailRewardComponentInfo.ItemInfoList.Add(itemInfo);
                }
            }

            mailInfo.MailRewardState = mailInfo.MailRewardComponentInfo.ItemInfoList.Count > 0 ? (int)MailRewardState.NotReceived : (int)MailRewardState.NotReward;

            return mailInfo;
        }

        private static async ETTask SendToPlayer(Scene scene, MailCenterComponent mailCenterComponent, MailUnitComponent mailUnitComponent, MailInfo mailInfo, string playerIdStr)
        {
            if (!long.TryParse(playerIdStr, out long unitId))
            {
                Log.Error($"玩家ID解析失败: {playerIdStr}");
                return;
            }

            mailUnitComponent.Children.TryGetValue(unitId, out Entity mailUnitEntity);
            MailUnit targetMailUnit = mailUnitEntity as MailUnit;

            if (targetMailUnit != null)
            {
                // 在线，直接领取邮件
                MailComponent mailComponent = targetMailUnit.GetComponent<MailComponent>();
                if (mailComponent != null)
                {
                    mailComponent.AddMail(mailInfo);
                    Mail2C_ReceiveMail message = Mail2C_ReceiveMail.Create();
                    message.MailInfo = mailInfo;
                    MapMessageHelper.SendToClient(scene, targetMailUnit.Id, message);
                }
            }
            else
            {
                // 不在线，先保存在邮件中心，待玩家上线时再领取邮件
                mailCenterComponent.CreateServerMail(mailInfo, (int)MailReceiveType.PlayerId, playerIdStr);
            }

            await ETTask.CompletedTask;
        }

        private static async ETTask SendToAllPlayers(Scene scene, MailCenterComponent mailCenterComponent, MailUnitComponent mailUnitComponent, MailInfo mailInfo)
        {
            ServerMail serverMail = mailCenterComponent.CreateServerMail(mailInfo, (int)MailReceiveType.All, string.Empty);

            // 在线，直接领取邮件
            foreach (Entity entity in mailUnitComponent.Children.Values)
            {
                MailUnit mailUnit = entity as MailUnit;

                if (mailUnit == null)
                {
                    continue;
                }

                mailUnit.GetComponent<MailComponent>().AddMail(mailInfo);
                serverMail.ReceivedPlayerIds.Add(mailUnit.Id);

                Mail2C_ReceiveMail message = Mail2C_ReceiveMail.Create();
                message.MailInfo = mailInfo;
                MapMessageHelper.SendToClient(scene, mailUnit.Id, message);
            }

            await ETTask.CompletedTask;
        }

        private static async ETTask SendToLowLevelPlayers(Scene scene, MailCenterComponent mailCenterComponent, MailUnitComponent mailUnitComponent, MailInfo mailInfo, string levelStr)
        {
            if (!int.TryParse(levelStr, out int targetLevel))
            {
                Log.Error($"等级解析失败: {levelStr}");
                return;
            }

            ServerMail serverMail = mailCenterComponent.CreateServerMail(mailInfo, (int)MailReceiveType.LessLv, levelStr);

            foreach (Entity entity in mailUnitComponent.Children.Values)
            {
                MailUnit mailUnit = entity as MailUnit;

                if (mailUnit == null)
                {
                    continue;
                }

                UserInfoComponent userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponent>(scene, mailUnit.Id);
                if (userInfoComponent.GetLv() >= targetLevel)
                {
                    continue;
                }

                mailUnit.GetComponent<MailComponent>().AddMail(mailInfo);
                serverMail.ReceivedPlayerIds.Add(mailUnit.Id);

                Mail2C_ReceiveMail message = Mail2C_ReceiveMail.Create();
                message.MailInfo = mailInfo;
                MapMessageHelper.SendToClient(scene, mailUnit.Id, message);
            }

            await ETTask.CompletedTask;
        }
    }
}
