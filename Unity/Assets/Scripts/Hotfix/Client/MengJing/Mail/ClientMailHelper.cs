using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(MailComponentC))]
    public static class ClientMailHelper
    {
        public static async ETTask<int> GetAllMail(Scene root)
        {
            C2Mail_GetAllMailList request = C2Mail_GetAllMailList.Create();

            Mail2C_GetAllMailList response = (Mail2C_GetAllMailList)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            MailComponentC mailComponentC = root.GetComponent<MailComponentC>();
            mailComponentC.Clear();
            foreach (MailInfo mailInfo in response.MailInfoList)
            {
                mailComponentC.AddMailFromMessage(mailInfo);
            }

            return response.Error;
        }

        public static async ETTask<int> OpeMail(Scene root, MailOpType mailOpType, List<long> mailIds)
        {
            C2Mail_OpeMail request = C2Mail_OpeMail.Create();
            request.MailOpType = (int)mailOpType;
            request.MailId.AddRange(mailIds);

            Mail2C_OpeMail response = (Mail2C_OpeMail)await root.GetComponent<ClientSenderComponent>().Call(request);

            MailComponentC mailComponent = root.GetComponent<MailComponentC>();
            foreach (MailInfo mailInfo in response.MailInfoList)
            {
                if (mailInfo.MailDeleteState == (int)MailDeleteState.Deleted)
                {
                    mailComponent.RemoveMail(mailInfo.Id);
                    continue;
                }

                mailComponent.UpdateMail(mailInfo);
            }

            EventSystem.Instance.Publish(root, new MailUpdate());

            return ErrorCode.ERR_Success;
        }
    }
}