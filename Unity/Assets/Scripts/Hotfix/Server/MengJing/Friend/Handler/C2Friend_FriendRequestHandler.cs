namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_FriendRequestHandler : MessageHandler<FriendUnit, C2Friend_FriendRequest, Friend2C_FriendRequest>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_FriendRequest request, Friend2C_FriendRequest response)
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

                friendUnitComponent.Children.TryGetValue(targetFrientUnitId, out Entity friendUnitEntity);
                FriendUnit targetFriendUnit = friendUnitEntity as FriendUnit;

                if (targetFriendUnit != null)
                {
                    // 在线
                    FriendComponentS targetFriendComponent = targetFriendUnit.GetComponent<FriendComponentS>();

                    if (targetFriendComponent.FriendList.Contains(targetFrientUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsFriend;
                        return;
                    }

                    if (targetFriendComponent.RequestList.Contains(targetFrientUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsRequest;
                        return;
                    }

                    myFriendComponent.RequestList.Add(targetFrientUnitId);
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

                    if (targetFriendComponent.FriendList.Contains(targetFrientUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsFriend;
                        return;
                    }

                    if (targetFriendComponent.RequestList.Contains(targetFrientUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsRequest;
                        return;
                    }

                    myFriendComponent.RequestList.Add(targetFrientUnitId);

                    await UnitCacheHelper.SaveComponent(scene, targetFriendComponent);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}