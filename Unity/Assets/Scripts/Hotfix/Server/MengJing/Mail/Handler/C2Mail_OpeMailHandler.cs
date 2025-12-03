namespace ET.Server
{
    [FriendOf(typeof(MailComponentS))]
    [FriendOf(typeof(Mail))]
    [MessageHandler(SceneType.Mail)]
    public class C2Mail_OpeMailHandler: MessageHandler<MailUnit, C2Mail_OpeMail, Mail2C_OpeMail>
    {
        protected override async ETTask Run(MailUnit mailUnit, C2Mail_OpeMail request, Mail2C_OpeMail response)
        {
            await ETTask.CompletedTask;
        }
    }
}