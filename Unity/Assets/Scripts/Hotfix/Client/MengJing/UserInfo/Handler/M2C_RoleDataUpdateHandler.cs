namespace ET.Client
{
    [FriendOf(typeof(UserInfoComponentC))]
    [MessageHandler(SceneType.Demo)]
    public class M2C_RoleDataUpdateHandler : MessageHandler<Scene, M2C_RoleDataUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_RoleDataUpdate message)
        {
            UserInfoComponentC userInfoComponent = root.GetComponent<UserInfoComponentC>();
            string oldString = null;
            long oldLong = 0;

            switch (message.UpdateType)
            {
                case (int)UserDataType.Gold:
                    oldLong = userInfoComponent.Gold;
                    userInfoComponent.Gold = message.UpdateValueLong;
                    break;
                case (int)UserDataType.Diamond:
                    oldLong = userInfoComponent.Diamond;
                    userInfoComponent.Diamond = message.UpdateValueLong;
                    break;
                case (int)UserDataType.Exp:
                    oldLong = userInfoComponent.Exp;
                    userInfoComponent.Exp = message.UpdateValueLong;
                    // EventSystem.Instance.Publish(root, new UpdateUserDataExp() { ChangeValue = changeValue, UpdateValue = message.UpdateValueLong });
                    // return;
                    break;
            }

            //更新比较频繁的单独处理
            EventSystem.Instance.Publish(root, new UpdateUserData()
            {
                UserDataType = (UserDataType)message.UpdateType,
                OldLong = oldLong,
                NewLong = message.UpdateValueLong,
                OldString = oldString,
                NewString = message.UpdateTypeValue
            });

            await ETTask.CompletedTask;
        }
    }
}