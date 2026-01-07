namespace ET.Client
{
    public static class ClientLotteryDrawHelper
    {
        public static async ETTask<M2C_LotteryDrawRequest> LotteryDrawRequest(Scene root, int opType, int lookingForwardHeroId)
        {
            C2M_LotteryDrawRequest request = C2M_LotteryDrawRequest.Create();
            request.OpType = opType;
            request.LookingForwardHeroId = lookingForwardHeroId;

            M2C_LotteryDrawRequest response = (M2C_LotteryDrawRequest)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response;
        }
    }
}