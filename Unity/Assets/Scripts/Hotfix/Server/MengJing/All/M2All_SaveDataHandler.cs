using System;

namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class M2All_SaveDataHandler : MessageHandler<Scene, M2All_SaveData, All2M_SaveData>
    {
        protected override async ETTask Run(Scene scene, M2All_SaveData request, All2M_SaveData response)
        {
            try
            {
                switch (scene.SceneType)
                {
                    case SceneType.Mail:
                    {
                        await UnitCacheHelper.SaveComponent(scene, scene.GetComponent<MailCenterComponent>());
                        foreach (Entity entity in scene.GetComponent<MailUnitComponent>().Children.Values)
                        {
                            MailUnit mailUnit = entity as MailUnit;

                            if (mailUnit == null)
                            {
                                continue;
                            }

                            await UnitCacheHelper.SaveComponent(scene, mailUnit.GetComponent<MailComponentS>());
                        }

                        Log.Info($"数据落地:  Mail: {scene.Zone()}");

                        break;
                    }
                    case SceneType.Friend:
                    {
                        foreach (Entity entity in scene.GetComponent<FriendUnitComponent>().Children.Values)
                        {
                            FriendUnit friendUnit = entity as FriendUnit;

                            if (friendUnit == null)
                            {
                                continue;
                            }

                            await UnitCacheHelper.SaveComponent(scene, friendUnit.GetComponent<FriendComponentS>());
                        }

                        Log.Info($"数据落地:  Friend: {scene.Zone()}");

                        break;
                    }
                    case SceneType.Chat:
                    {
                        await UnitCacheHelper.SaveComponent(scene, scene.GetComponent<ChatCenterComponent>());
                        foreach (Entity entity in scene.GetComponent<ChatUnitComponent>().Children.Values)
                        {
                            ChatUnit chatUnit = entity as ChatUnit;

                            if (chatUnit == null)
                            {
                                continue;
                            }

                            await UnitCacheHelper.SaveComponent(scene, chatUnit.GetComponent<ChatComponentS>());
                        }

                        Log.Info($"数据落地:  Chat: {scene.Zone()}");

                        break;
                    }
                    case SceneType.Rank:
                    {
                        await UnitCacheHelper.SaveComponent(scene, scene.GetComponent<RankCenterComponent>());

                        Log.Info($"数据落地:  Rank: {scene.Zone()}");

                        break;
                    }
                    default:
                        break;
                }

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}