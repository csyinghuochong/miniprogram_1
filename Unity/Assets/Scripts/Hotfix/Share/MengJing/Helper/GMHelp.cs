using System.Collections.Generic;

namespace ET
{
    public static class GMHelp
    {

        //qqUID_84E70C11CC937F72EE508CD43D7DD4DA һֱ��ͷ��
        public static bool IsGmAccount(string account)
        {
            return GMData.GmAccount.Contains(account);
        }

        
        public static int GetRandomNumber()
        {
            return 2;
        }
    }
}
