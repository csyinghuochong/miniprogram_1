using System.Collections.Generic;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_CreateRoleDataHandler: MessageSessionHandler<C2R_CreateRoleData, R2C_CreateRoleData>
    {
	   
        protected override async ETTask Run(Session session, C2R_CreateRoleData request, R2C_CreateRoleData response)
        {
	        if (session.GetComponent<SessionLockingComponent>()!=null)
	        { 
		        response.Error = ErrorCode.ERR_RequestRepeatedly;
		        response.Message = "角色名字过短!";
		        return;
	        }

	        if (session.Root().SceneType != SceneType.Realm)
	        {
		        Log.Error($"LoginTest C2G_CreateRoleData请求的Scene错误，当前Scene为：{session.Root().SceneType}");
		        session.Dispose();
		        return;
	        }
	        
	        Log.Debug("C2R_CreateRoleDataHandler.server1");
            //判断名字是否符合要求
            if (string.IsNullOrEmpty(request.CreateName))
            {
                response.Error = ErrorCode.ERR_CreateRoleName;
                response.Message = "角色名字过短!";
                return;
            }
            if (request.CreateName.Length >= 8)
            {
                response.Error = ErrorCode.ERR_CreateRoleName;
                response.Message = "角色名字过长!";
                return;
            }
            if (session.Zone() == 0)
            {
                Log.Error("session.DomainZone() == 0");
                response.Error = ErrorCode.ERR_Error;
                return;
            }
            
            using (session.AddComponent<SessionLockingComponent>())
			{
				using (await session.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginAccount, request.AccountId.GetHashCode()))
				{
					DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(request.ServerId);

					int zone = session.Zone();
					dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(zone);
			        List<DBCenterAccountInfo> newAccountList = await dbComponent.Query<DBCenterAccountInfo>(zone, d => d.Id == request.AccountId);
			        DBCenterAccountInfo newAccount = newAccountList[0];

					int robotId = 0;
					if (newAccount.Password == ConfigData.RobotPassWord)
					{
						robotId = int.Parse(newAccount.Account.Split('_')[1]);
					}

					//存储账号信息
					CreateRoleInfo createRoleInfo = CreateRoleInfo.Create();
					createRoleInfo.UnitId = IdGenerater.Instance.GenerateId();
					createRoleInfo.PlayerLv = 1;
					createRoleInfo.PlayerOcc = request.CreateOcc;
					createRoleInfo.PlayerName = request.CreateName;
					createRoleInfo.RobotId = robotId;
					createRoleInfo.ServerId = request.ServerId;
					newAccount.RoleList.Add(createRoleInfo);
					await dbComponent.Save(newAccount);

					//返回角色信息
					response.createRoleInfo = createRoleInfo;
				}
			}
            
            await ETTask.CompletedTask;
        }
    }
}
	