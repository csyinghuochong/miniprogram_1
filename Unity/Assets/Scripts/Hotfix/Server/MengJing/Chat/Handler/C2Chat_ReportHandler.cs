namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_ReportHandler : MessageHandler<ChatUnit, C2Chat_Report, Chat2C_Report>
    {
        protected override async ETTask Run(ChatUnit chatUnit, C2Chat_Report request, Chat2C_Report response)
        {
            Scene root = chatUnit.Root();
            using (await root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Chat, chatUnit.Id))
            {
                ChatUnitComponent chatUnitComponent = root.GetComponent<ChatUnitComponent>();

                long myUnitId = chatUnit.Id;
                long targetUnitId = request.UnitId;

                chatUnitComponent.Children.TryGetValue(targetUnitId, out Entity chatUnitEntity);
                ChatUnit targetChatUnit = chatUnitEntity as ChatUnit;

                if (targetChatUnit != null)
                {
                    ChatComponent targetChatComponent = targetChatUnit.GetComponent<ChatComponent>();
                    if (!targetChatComponent.ReportList.Contains(myUnitId))
                    {
                        targetChatComponent.ReportList.Add(myUnitId);
                    }

                    // 禁言
                    if (targetChatComponent.ReportList.Count == ConfigData.ChatReportMax)
                    {
                        targetChatComponent.UnmuteTime = TimeHelper.ServerNow() + TimeHelper.OneDay;
                    }
                }
                else
                {
                    ChatComponent targetChatComponent = await UnitCacheHelper.GetComponent<ChatComponent>(root, targetUnitId);
                    if (!targetChatComponent.ReportList.Contains(myUnitId))
                    {
                        targetChatComponent.ReportList.Add(myUnitId);
                    }

                    // 禁言
                    if (targetChatComponent.ReportList.Count == ConfigData.ChatReportMax)
                    {
                        targetChatComponent.UnmuteTime = TimeHelper.ServerNow() + TimeHelper.OneDay;
                    }

                    await UnitCacheHelper.SaveComponent(root, targetChatComponent);
                }
            }
        }
    }
}