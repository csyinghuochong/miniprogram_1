using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ArchiveComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ReceivedArchiveRewardHandler : MessageLocationHandler<Unit, C2M_ReceivedArchiveReward, M2C_ReceivedArchiveReward>
    {
        protected override async ETTask Run(Unit unit, C2M_ReceivedArchiveReward request, M2C_ReceivedArchiveReward response)
        {
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();
            ArchiveComponent archiveComponent = unit.GetComponent<ArchiveComponent>();

            if (!ArchiveRewardConfigCategory.Instance.DataMap.ContainsKey(request.RewardId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            if (archiveComponent.ReceivedArchiveRewardIds.Contains(request.RewardId))
            {
                response.Error = ErrorCode.ERR_AlreadyReceived;
                return;
            }

            int currentPoint = archiveComponent.GetCurrentPoint();
            ArchiveRewardConfig archiveRewardConfig = ArchiveRewardConfigCategory.Instance.DataMap[request.RewardId];

            if (currentPoint < archiveRewardConfig.RequiredPoints)
            {
                response.Error = ErrorCode.ERR_NotEnoughPoint;
                return;
            }

            archiveComponent.ReceivedArchiveRewardIds.Add(request.RewardId);

            List<RewardItem> rewardItems = new();
            foreach (RewardItem rewardItem in archiveRewardConfig.RewardItem)
            {
                rewardItems.Add(rewardItem);
            }
            inventoryComponent.AddItemData(rewardItems);

            await ETTask.CompletedTask;
        }
    }
}