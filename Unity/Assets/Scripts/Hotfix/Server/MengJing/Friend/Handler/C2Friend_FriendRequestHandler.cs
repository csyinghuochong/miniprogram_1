namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_FriendRequestHandler : MessageHandler<FriendUnit, C2Friend_FriendRequest, Friend2C_FriendRequest>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_FriendRequest request, Friend2C_FriendRequest response)
        {
            Scene root = friendUnit.Root();

            using (await root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Friend, friendUnit.Id))
            {
                FriendUnitComponent friendUnitComponent = root.GetComponent<FriendUnitComponent>();
                FriendComponentS myFriendComponent = friendUnit.GetComponent<FriendComponentS>();

                long myUnitId = friendUnit.Id;
                long targetFrientUnitId = request.UnitId;

                if (targetFrientUnitId == friendUnit.Id)
                {
                    response.Error = ErrorCode.ERR_FriendIsSelf;
                    return;
                }

                if (myFriendComponent.FriendList.Contains(targetFrientUnitId))
                {
                    response.Error = ErrorCode.ERR_FriendIsFriend;
                    return;
                }

                friendUnitComponent.Children.TryGetValue(targetFrientUnitId, out Entity friendUnitEntity);
                FriendUnit targetFriendUnit = friendUnitEntity as FriendUnit;

                if (targetFriendUnit != null)
                {
                    // 在线
                    FriendComponentS targetFriendComponent = targetFriendUnit.GetComponent<FriendComponentS>();

                    if (targetFriendComponent.FriendList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsFriend;
                        return;
                    }

                    if (targetFriendComponent.RequestList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsRequest;
                        return;
                    }

                    if (targetFriendComponent.BlackList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsBlack;
                        return;
                    }

                    targetFriendComponent.RequestList.Add(myUnitId);

                    Friend2C_ReceiveFriendRequest message = Friend2C_ReceiveFriendRequest.Create();
                    message.FriendDataInfo = await FriendHelper.GetFriendDataInfo(root, myUnitId);
                    MapMessageHelper.SendToClient(root, targetFriendUnit.Id, message);
                }
                else
                {
                    // 离线
                    FriendComponentS targetFriendComponent = await UnitCacheHelper.GetComponent<FriendComponentS>(root, targetFrientUnitId);

                    if (targetFriendComponent == null)
                    {
                        response.Error = ErrorCode.ERR_ComponentIsNull;
                        return;
                    }

                    if (targetFriendComponent.FriendList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsFriend;
                        return;
                    }

                    if (targetFriendComponent.RequestList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsRequest;
                        return;
                    }

                    if (targetFriendComponent.BlackList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsBlack;
                        return;
                    }

                    targetFriendComponent.RequestList.Add(myUnitId);

                    await UnitCacheHelper.SaveComponent(root, targetFriendComponent);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}