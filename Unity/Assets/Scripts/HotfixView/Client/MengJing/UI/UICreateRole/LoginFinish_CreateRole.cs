using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class LoginFinish_CreateRole : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            PlayerInfoComponent playerInfoComponent = scene.GetComponent<PlayerInfoComponent>();

            if (playerInfoComponent.CreateRoleList.Count > 0)
            {
                // 直接进入游戏
                CreateRoleInfo selected = playerInfoComponent.CreateRoleList[0];

                PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastUserID, selected.UnitId.ToString());
                playerInfoComponent.CurrentRoleId = selected.UnitId;
                scene.GetComponent<MapComponent>().MapType = MapTypeEnum.LoginScene;

                int errorCode = await LoginHelper.LoginGameAsync(scene, 0);

                if (errorCode == ErrorCode.ERR_SessionDisconnect)
                {
                    Log.Warning("网络断线，请重新登陆！");
                }
            }
            else
            {
                // 创建角色界面
                await scene.GetComponent<UIComponent>().Create(UIType.UICreateRole);
            }

            scene.GetComponent<UIComponent>().Remove(UIType.UILogin);

            await ETTask.CompletedTask;
        }
    }
}