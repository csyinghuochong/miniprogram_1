namespace ET.Server
{
    [FriendOf(typeof(MailComponentS))]
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailRewardComponent))]
    [MessageHandler(SceneType.Mail)]
    public class C2Mail_OpeMailHandler : MessageHandler<MailUnit, C2Mail_OpeMail, Mail2C_OpeMail>
    {
        protected override async ETTask Run(MailUnit mailUnit, C2Mail_OpeMail request, Mail2C_OpeMail response)
        {
            using (await mailUnit.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.OpeMail, mailUnit.Id))
            {
                MailComponentS mailComponent = mailUnit.GetComponent<MailComponentS>();
                foreach (long mailId in request.MailId)
                {
                    mailComponent.Children.TryGetValue(mailId, out Entity mailEntity);

                    Mail mail = mailEntity as Mail;

                    if (mail == null)
                    {
                        response.Error = ErrorCode.ERR_MailNotExist;
                        return;
                    }

                    if (mail.MailDeleteState == (int)MailDeleteState.Deleted)
                    {
                        response.Error = ErrorCode.ERR_MailDeleted;
                        return;
                    }

                    if (mail.EndTime >= TimeInfo.Instance.ServerNow())
                    {
                        response.Error = ErrorCode.ERR_MailTimeOut;
                        return;
                    }

                    if (request.MailOpType == (int)MailOpType.Read)
                    {
                        mail.MailReadState = (int)MailReadState.Read;
                    }
                    else if (request.MailOpType == (int)MailOpType.Received)
                    {
                        if (mail.MailRewardState == (int)MailRewardState.NotReward)
                        {
                            response.Error = ErrorCode.ERR_MailNotReward;
                            return;
                        }

                        if (mail.MailRewardState == (int)MailRewardState.Received)
                        {
                            response.Error = ErrorCode.ERR_MailRewardAlreadyReceived;
                            return;
                        }

                        // 发送邮件奖励
                        Mail2M_ReceiveReward mail2MReceiveReward = Mail2M_ReceiveReward.Create();
                        foreach (Item item in mail.GetComponent<MailRewardComponent>().ItemList)
                        {
                            mail2MReceiveReward.ItemInfoList.Add(item.ToMessage());
                        }

                        M2Mail_ReceiveReward m2MailReceiveReward = (M2Mail_ReceiveReward)await mailUnit.Root()
                                .GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Call(mailUnit.Id, mail2MReceiveReward);
                        if (m2MailReceiveReward.Error != ErrorCode.ERR_Success)
                        {
                            response.Error = m2MailReceiveReward.Error;
                            return;
                        }

                        mail.MailReadState = (int)MailReadState.Read;
                        mail.MailRewardState = (int)MailRewardState.Received;
                    }
                    else if (request.MailOpType == (int)MailOpType.Delete)
                    {
                        mail.MailDeleteState = (int)MailDeleteState.Deleted;
                    }
                }
            }

            await ETTask.CompletedTask;
        }
    }
}