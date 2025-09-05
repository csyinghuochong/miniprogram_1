using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UILoginComponent : Entity, IAwake
    {
        public ServerItem ServerInfo;
        public long LastLoginTime;

        public Text Text_SelectServerName;
        public InputField InputField_Account;
        public InputField InputField_Password;
        public Button Button_Login;
    }
}