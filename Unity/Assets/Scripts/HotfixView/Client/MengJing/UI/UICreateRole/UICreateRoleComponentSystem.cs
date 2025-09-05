using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UICreateRoleComponent))]
    [FriendOf(typeof(UICreateRoleComponent))]
    public static partial class UICreateRoleComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UICreateRoleComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.InputField_RoleName = rc.Get<GameObject>("InputField_RoleName").GetComponent<InputField>();
            self.Button_Create = rc.Get<GameObject>("Button_Create").GetComponent<Button>();

            self.Button_Create.onClick.AddListener(() => { self.OnCreateRole().Coroutine(); });
        }

        public static async ETTask OnCreateRole(this UICreateRoleComponent self)
        {
            if (Time.time - self.LastLoginTime < 4f)
            {
                return;
            }

            PlayerInfoComponent playerInfoComponent = self.Root().GetComponent<PlayerInfoComponent>();

            if (string.IsNullOrEmpty(self.InputField_RoleName.text))
            {
                return;
            }

            int error = await LoginHelper.RequestCreateRole(self.Root(), playerInfoComponent.AccountId, 0, self.InputField_RoleName.text);

            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            if (playerInfoComponent.CreateRoleList.Count == 0)
            {
                Log.Debug("需要先创建角色！");
                return;
            }

            CreateRoleInfo selected = playerInfoComponent.CreateRoleList[0];

            self.LastLoginTime = Time.time;
            PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastUserID, selected.UnitId.ToString());
            playerInfoComponent.CurrentRoleId = selected.UnitId;
            self.Root().GetComponent<MapComponent>().MapType = MapTypeEnum.LoginScene;

            int errorCode = await LoginHelper.LoginGameAsync(self.Root(), 0);

            if (errorCode == ErrorCode.ERR_SessionDisconnect)
            {
                Log.Warning("网络断线，请重新登陆！");
            }
        }
    }
}