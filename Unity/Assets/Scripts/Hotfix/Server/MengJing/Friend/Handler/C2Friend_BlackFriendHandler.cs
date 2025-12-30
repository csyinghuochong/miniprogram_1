namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_BlackFriendHandler : MessageHandler<FriendUnit, C2Friend_BlackFriend, Friend2C_BlackFriend>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_BlackFriend request, Friend2C_BlackFriend response)
        {
            Scene root = friendUnit.Root();
            using (await root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Friend, friendUnit.Id))
            {
                FriendComponentS myFriendComponent = friendUnit.GetComponent<FriendComponentS>();

                long myUnitId = friendUnit.Id;
                long targetUnitId = request.UnitId;

                if (targetUnitId == friendUnit.Id)
                {
                    response.Error = ErrorCode.ERR_FriendIsSelf;
                    return;
                }

                FriendComponentS targetFriendComponent = await UnitCacheHelper.GetComponent<FriendComponentS>(root, targetUnitId);
                if (targetFriendComponent == null)
                {
                    response.Error = ErrorCode.ERR_TargetUnitIsNull;
                    return;
                }

                if (request.Ope == 0)
                {
                    // 拉黑

                    if (myFriendComponent.FriendList.Contains(targetUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendCantBlack;
                        return;
                    }

                    if (myFriendComponent.BlackList.Contains(targetUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsBlack;
                        return;
                    }

                    if (myFriendComponent.RequestList.Contains(targetUnitId))
                    {
                        myFriendComponent.RequestList.Remove(targetUnitId);
                    }

                    if (targetFriendComponent.RequestList.Contains(myUnitId))
                    {
                        targetFriendComponent.RequestList.Remove(myUnitId);
                    }

                    myFriendComponent.BlackList.Add(targetUnitId);

                    response.FriendDataInfo = await FriendHelper.GetFriendDataInfo(root, targetUnitId);
                }
                else
                {
                    // 取消拉黑

                    if (!myFriendComponent.BlackList.Contains(targetUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsNotBlack;
                        return;
                    }

                    myFriendComponent.BlackList.Remove(targetUnitId);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}