namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_FriendRequestAcceptHandler : MessageHandler<FriendUnit, C2Friend_FriendRequestAccept, Friend2C_FriendRequestAccept>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_FriendRequestAccept request, Friend2C_FriendRequestAccept response)
        {
            Scene root = friendUnit.Scene();
            using (await root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Friend, friendUnit.Id))
            {
                FriendUnitComponent friendUnitComponent = root.GetComponent<FriendUnitComponent>();
                FriendComponentS myFriendComponent = friendUnit.GetComponent<FriendComponentS>();

                long myUnitId = friendUnit.Id;
                long targetFrientUnitId = request.UnitId;

                if (myFriendComponent.FriendList.Contains(targetFrientUnitId))
                {
                    response.Error = ErrorCode.ERR_FriendIsFriend;
                    return;
                }

                if (!myFriendComponent.RequestList.Contains(targetFrientUnitId))
                {
                    response.Error = ErrorCode.ERR_FriendIsNotRequest;
                    return;
                }

                if (myFriendComponent.BlackList.Contains(targetFrientUnitId))
                {
                    response.Error = ErrorCode.ERR_FriendIsBlack;
                    return;
                }

                if (request.IsAgree == 1)
                {
                    // 同意
                    friendUnitComponent.Children.TryGetValue(targetFrientUnitId, out Entity friendUnitEntity);
                    FriendUnit targetFriendUnit = friendUnitEntity as FriendUnit;

                    if (targetFriendUnit != null)
                    {
                        // 在线
                        FriendComponentS targetFriendComponent = targetFriendUnit.GetComponent<FriendComponentS>();

                        targetFriendComponent.FriendList.Add(myUnitId);
                        if (targetFriendComponent.RequestList.Contains(myUnitId))
                        {
                            targetFriendComponent.RequestList.Remove(myUnitId);
                        }

                        myFriendComponent.FriendList.Add(targetFrientUnitId);
                        myFriendComponent.RequestList.Remove(targetFrientUnitId);

                        Friend2C_FriendRequestSucceed message = Friend2C_FriendRequestSucceed.Create();
                        message.FriendDataInfo = await FriendHelper.GetFriendDataInfo(root, myUnitId);
                        MapMessageHelper.SendToClient(root, targetFriendUnit.Id, message);
                    }
                    else
                    {
                        // 离线
                        FriendComponentS targetFriendComponent = await UnitCacheHelper.GetComponent<FriendComponentS>(root, request.UnitId);

                        if (targetFriendComponent == null)
                        {
                            response.Error = ErrorCode.ERR_ComponentIsNull;
                            return;
                        }

                        targetFriendComponent.FriendList.Add(myUnitId);
                        if (targetFriendComponent.RequestList.Contains(myUnitId))
                        {
                            targetFriendComponent.RequestList.Remove(myUnitId);
                        }

                        myFriendComponent.FriendList.Add(targetFrientUnitId);
                        myFriendComponent.RequestList.Remove(targetFrientUnitId);

                        await UnitCacheHelper.SaveComponent(root, targetFriendComponent);
                    }

                    // 创建聊天室
                    Friend2Chat_CreateChatRoom friend2ChatCreateChatRoom = Friend2Chat_CreateChatRoom.Create();
                    friend2ChatCreateChatRoom.UnitId_1 = myUnitId;
                    friend2ChatCreateChatRoom.UnitId_2 = targetFrientUnitId;

                    await root.GetComponent<MessageSender>().Call(UnitCacheHelper.GetChatServerId(root.Zone()), friend2ChatCreateChatRoom);
                }
                else
                {
                    // 拒绝
                    myFriendComponent.RequestList.Remove(targetFrientUnitId);
                }
            }
        }
    }
}