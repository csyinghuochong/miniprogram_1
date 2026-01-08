namespace ET.Server
{
    [FriendOf(typeof(MailComponent))]
    [FriendOf(typeof(Mail))]
    [MessageHandler(SceneType.Mail)]
    public class C2Mail_GetAllMailListHandler : MessageHandler<MailUnit, C2Mail_GetAllMailList, Mail2C_GetAllMailList>
    {
        protected override async ETTask Run(MailUnit mailUnit, C2Mail_GetAllMailList request, Mail2C_GetAllMailList response)
        {
            MailComponent mailComponent = mailUnit.GetComponent<MailComponent>();

            foreach (Mail mail in mailComponent.MailList)
            {
                if (mail.MailDeleteState == (int)MailDeleteState.Deleted)
                {
                    continue;
                }

                if (mail.EndTime < TimeHelper.ServerNow())
                {
                    continue;
                }

                response.MailInfoList.Add(mail.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}