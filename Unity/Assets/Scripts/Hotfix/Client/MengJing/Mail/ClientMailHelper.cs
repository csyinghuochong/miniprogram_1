using System.Collections.Generic;

namespace ET.Client
{
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
    }
}