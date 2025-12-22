namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class G2Chat_ExitChatServerHandler : MessageLocationHandler<ChatUnit, G2Chat_ExitChatServer, Chat2G_ExitChatServer>
    {
        protected override async ETTask Run(ChatUnit chatUnit, G2Chat_ExitChatServer request, Chat2G_ExitChatServer response)
        {
            ChatUnitExit(chatUnit).Coroutine();

            await ETTask.CompletedTask;
        }

        private async ETTask ChatUnitExit(ChatUnit chatUnit)
        {
            await chatUnit.Fiber().WaitFrameFinish();
            await chatUnit.RemoveLocation(LocationType.Chat);
            chatUnit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Remove(chatUnit.Id);
            chatUnit?.Dispose();

            await ETTask.CompletedTask;
        }
    }
}