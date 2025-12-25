namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_DeleteFriendHandler : MessageHandler<FriendUnit, C2Friend_DeleteFriend, Friend2C_DeleteFriend>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_DeleteFriend request, Friend2C_DeleteFriend response)
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

                if (!myFriendComponent.FriendList.Contains(targetFrientUnitId))
                {
                    response.Error = ErrorCode.ERR_FriendIsNotFriend;
                    return;
                }

                friendUnitComponent.Children.TryGetValue(targetFrientUnitId, out Entity friendUnitEntity);
                FriendUnit targetFriendUnit = friendUnitEntity as FriendUnit;

                if (targetFriendUnit != null)
                {
                    // 在线
                    FriendComponentS targetFriendComponent = targetFriendUnit.GetComponent<FriendComponentS>();

                    if (!targetFriendComponent.FriendList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsNotFriend;
                        return;
                    }

                    targetFriendComponent.FriendList.Remove(myUnitId);
                    myFriendComponent.FriendList.Remove(targetFrientUnitId);

                    Friend2C_DeleteYou message = Friend2C_DeleteYou.Create();
                    message.UnitId = myUnitId;
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

                    if (!targetFriendComponent.FriendList.Contains(myUnitId))
                    {
                        response.Error = ErrorCode.ERR_FriendIsNotFriend;
                        return;
                    }

                    targetFriendComponent.RequestList.Remove(myUnitId);
                    myFriendComponent.FriendList.Remove(targetFrientUnitId);

                    await UnitCacheHelper.SaveComponent(root, targetFriendComponent);
                }
            }
        }
    }
}