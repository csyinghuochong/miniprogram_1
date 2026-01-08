namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    [FriendOf(typeof(ChatUnit))]
    public class G2Chat_LoginChatServerHandler : MessageHandler<Scene, G2Chat_LoginChatServer, Chat2G_LoginChatServer>
    {
        protected override async ETTask Run(Scene scene, G2Chat_LoginChatServer request, Chat2G_LoginChatServer response)
        {
            ChatUnitComponent chatUnitComponent = scene.Root().GetComponent<ChatUnitComponent>();
            chatUnitComponent.Children.TryGetValue(request.UnitId, out Entity chatUnitEntity);

            ChatUnit chatUnit = chatUnitEntity as ChatUnit;

            if (chatUnit != null)
            {
                return;
            }

            chatUnit = chatUnitComponent.AddChildWithId<ChatUnit>(request.UnitId);
            chatUnit.Name = request.Name;

            ChatComponent chatComponent = await UnitCacheHelper.GetComponent<ChatComponent>(scene, request.UnitId);
            if (chatComponent == null)
            {
                chatComponent = chatUnit.AddComponent<ChatComponent>();
            }
            else
            {
                chatUnit.AddComponent(chatComponent);
            }

            chatUnit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            await chatUnit.AddLocation(LocationType.Chat);

            await ETTask.CompletedTask;
        }
    }
}