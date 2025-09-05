using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILoginComponent))]
    [FriendOf(typeof(UILoginComponent))]
    public static partial class UILoginComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILoginComponent self)
        {
            CommonViewHelper.TargetFrameRate(60);
            GameObject.Find("Global").GetComponent<Init>().TogglePatchWindow(false);

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_SelectServerName = rc.Get<GameObject>("Text_SelectServerName").GetComponent<Text>();
            self.InputField_Account = rc.Get<GameObject>("InputField_Account").GetComponent<InputField>();
            self.InputField_Password = rc.Get<GameObject>("InputField_Password").GetComponent<InputField>();
            self.Button_Login = rc.Get<GameObject>("Button_Login").GetComponent<Button>();

            self.Button_Login.onClick.AddListener(() => { self.OnLoginButton().Coroutine(); });

            self.InputField_Account.text = "test01";
            self.InputField_Password.text = "123";
            
            self.RequestServerList().Coroutine();
        }

        private static async ETTask OnLoginButton(this UILoginComponent self)
        {
            if (TimeHelper.ServerNow() - self.LastLoginTime < 3000)
            {
                return;
            }

            if (self.ServerInfo == null)
            {
                self.RequestServerList().Coroutine();
                return;
            }

            self.LastLoginTime = TimeHelper.ServerNow();
            PlayerInfoComponent playerInfoComponent = self.Root().GetComponent<PlayerInfoComponent>();
            playerInfoComponent.ServerItem = self.ServerInfo;
            playerInfoComponent.Account = self.InputField_Account.text.ToLower();
            playerInfoComponent.Password = self.InputField_Password.text.ToLower();
            playerInfoComponent.VersionMode = GlobalHelp.GetVersionMode();

            PlayerPrefsHelp.SetInt(PlayerPrefsHelp.MyServerID, self.ServerInfo.ServerId);
            PlayerPrefsHelp.SetOldServerIds(self.ServerInfo.ServerId);
            // PlayerPrefsHelp.SetString("MJ_Account", self.InputField_Account.text);
            // PlayerPrefsHelp.SetString("MJ_Password", self.InputField_Password.text);

            int error = await LoginHelper.Login(self.Root(), self.InputField_Account.text, self.InputField_Password.text, 0,
                GlobalHelp.GetVersionMode());
            Log.Debug($"LoginHelper.Login:  {error}");
        }

        private static async ETTask RequestServerList(this UILoginComponent self)
        {
            //获取服务器列表
            R2C_ServerList r2CServerList = await LoginHelper.GetServerList(self.Root(), GlobalHelp.GetVersionMode());
            if (r2CServerList == null || r2CServerList.ServerItems.Count == 0)
            {
                return;
            }

            self.CheckServerList(r2CServerList.ServerItems, GlobalHelp.GetVersionMode());
            ServerItem serverItem = r2CServerList.ServerItems[r2CServerList.ServerItems.Count - 1];
            PlayerInfoComponent playerInfoComponent = self.Root().GetComponent<PlayerInfoComponent>();
            playerInfoComponent.ServerItem = serverItem;
            playerInfoComponent.AllServerList = r2CServerList.ServerItems;

            int myserver = PlayerPrefsHelp.GetInt(PlayerPrefsHelp.MyServerID);
            myserver = playerInfoComponent.GetNewServerId(myserver);

            for (int i = 0; i < r2CServerList.ServerItems.Count; i++)
            {
                if (r2CServerList.ServerItems[i].ServerId == myserver)
                {
                    serverItem = r2CServerList.ServerItems[i];
                    break;
                }
            }

            self.OnSelectServer(serverItem);
            //如果之前登陆过游戏，记录一下服务器id. serverItem = ServerHelper.GetServerItem(oldid);
        }

        private static void CheckServerList(this UILoginComponent self, List<ServerItem> serverItems, int versionMode)
        {
            for (int i = serverItems.Count - 1; i >= 0; i--)
            {
                if (versionMode == VersionMode.BanHao)
                {
                    if (!CommonHelp.IsBanHaoZone(serverItems[i].ServerId))
                    {
                        serverItems.RemoveAt(i);
                        continue;
                    }
                }

                if (versionMode == VersionMode.Beta)
                {
                    if (CommonHelp.IsBanHaoZone(serverItems[i].ServerId))
                    {
                        serverItems.RemoveAt(i);
                    }
                }
            }
        }

        public static void OnSelectServer(this UILoginComponent self, ServerItem serverId)
        {
            self.ServerInfo = serverId;
            self.Text_SelectServerName.text = serverId.ServerName;
        }
    }
}