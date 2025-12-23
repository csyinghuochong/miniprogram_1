namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    [FriendOf(typeof(ChatUnit))]
    public class G2Chat_LoginChatServerHandler : MessageHandler<Scene, G2Chat_LoginChatServer, Chat2G_LoginChatServer>
    {
        protected override async ETTask Run(Scene scene, G2Chat_LoginChatServer request, Chat2G_LoginChatServer response)
        {
            ChatUnitComponent chatInfoUnitsComponent = scene.Root().GetComponent<ChatUnitComponent>();
            chatInfoUnitsComponent.Children.TryGetValue(request.UnitId, out Entity chatUnitEntity);

            ChatUnit chatUnit = chatUnitEntity as ChatUnit;

            if (chatUnit != null)
            {
                return;
            }

            chatUnit = chatInfoUnitsComponent.AddChildWithId<ChatUnit>(request.UnitId);

            chatUnit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            await chatUnit.AddLocation(LocationType.Chat);

            await ETTask.CompletedTask;
        }
    }
}