using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UILoginComponent : Entity, IAwake
    {
        public ServerItem ServerInfo;
        public long LastLoginTime;

        public TMP_Text Text_SelectServerName;
        public TMP_InputField InputField_Account;
        public TMP_InputField InputField_Password;
        public Button Button_Login;
    }
}