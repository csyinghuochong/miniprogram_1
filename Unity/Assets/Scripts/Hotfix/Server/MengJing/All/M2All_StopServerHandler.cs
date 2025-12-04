using System;

namespace ET.Server
{
    [MessageHandler(SceneType.All)]
    public class M2All_StopServerHandler : MessageHandler<Scene, M2All_StopServer, All2M_StopServer>
    {
        protected override async ETTask Run(Scene scene, M2All_StopServer request, All2M_StopServer response)
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

                        Log.Debug($"数据落地:  Mail: {scene.Zone()}");

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