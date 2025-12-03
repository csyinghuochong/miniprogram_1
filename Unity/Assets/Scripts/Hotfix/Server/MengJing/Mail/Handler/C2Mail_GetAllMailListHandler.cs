namespace ET.Server
{
    [FriendOf(typeof(MailComponentS))]
    [FriendOf(typeof(Mail))]
    [MessageHandler(SceneType.Mail)]
    public class C2Mail_GetAllMailListHandler : MessageHandler<MailUnit, C2Mail_GetAllMailList, Mail2C_GetAllMailList>
    {
        protected override async ETTask Run(MailUnit mailUnit, C2Mail_GetAllMailList request, Mail2C_GetAllMailList response)
        {
            MailComponentS mailComponentS = mailUnit.GetComponent<MailComponentS>();

            foreach (Mail mail in mailComponentS.MailList)
            {
                if (mail.MailDeleteState == (int)MailDeleteState.Deleted)
                {
                    continue;
                }

                if (mail.EndTime >= TimeHelper.ServerNow())
                {
                    continue;
                }

                response.MailInfoList.Add(mail.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}