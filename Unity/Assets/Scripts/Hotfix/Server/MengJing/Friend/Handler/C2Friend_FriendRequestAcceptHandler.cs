namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_FriendRequestAcceptHandler : MessageHandler<FriendUnit, C2Friend_FriendRequestAccept, Friend2C_FriendRequestAccept>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_FriendRequestAccept request, Friend2C_FriendRequestAccept response)
        {
            Scene scene = friendUnit.Scene();
            using (await scene.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Friend, friendUnit.Id))
            {
                FriendUnitComponent friendUnitComponent = scene.GetComponent<FriendUnitComponent>();
                FriendComponentS myFriendComponent = friendUnit.GetComponent<FriendComponentS>();

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

                if (request.OpType == 0)
                {
                    // 同意
                    friendUnitComponent.Children.TryGetValue(targetFrientUnitId, out Entity friendUnitEntity);
                    FriendUnit targetFriendUnit = friendUnitEntity as FriendUnit;

                    if (targetFriendUnit != null)
                    {
                        // 在线
                        FriendComponentS targetFriendComponent = targetFriendUnit.GetComponent<FriendComponentS>();

                        targetFriendComponent.FriendList.Add(friendUnit.Id);

                        myFriendComponent.FriendList.Add(targetFrientUnitId);
                        myFriendComponent.RequestList.Remove(targetFrientUnitId);
                    }
                    else
                    {
                        // 离线
                        FriendComponentS targetFriendComponent = await UnitCacheHelper.GetComponent<FriendComponentS>(scene, request.UnitId);

                        if (targetFriendComponent == null)
                        {
                            response.Error = ErrorCode.ERR_ComponentIsNull;
                            return;
                        }

                        targetFriendComponent.FriendList.Add(friendUnit.Id);

                        myFriendComponent.FriendList.Add(targetFrientUnitId);
                        myFriendComponent.RequestList.Remove(targetFrientUnitId);

                        await UnitCacheHelper.SaveComponent(scene, targetFriendComponent);
                    }
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