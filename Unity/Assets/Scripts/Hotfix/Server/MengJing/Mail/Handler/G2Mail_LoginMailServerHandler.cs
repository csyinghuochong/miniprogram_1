namespace ET.Server.Handler
{
    [MessageHandler(SceneType.Mail)]
    public class G2Mail_LoginMailServerHandler : MessageHandler<Scene, G2Mail_LoginMailServer, Mail2G_LoginMailServer>
    {
        protected override async ETTask Run(Scene scene, G2Mail_LoginMailServer request, Mail2G_LoginMailServer response)
        {
            MailUnitComponent mailUnitComponent = scene.GetComponent<MailUnitComponent>();
            mailUnitComponent.Children.TryGetValue(request.UnitId, out Entity mailUnitEntity);

            MailUnit mailUnit = mailUnitEntity as MailUnit;

            if (mailUnit != null)
            {
                return;
            }

            mailUnit = mailUnitComponent.AddChildWithId<MailUnit>(request.UnitId);
            mailUnit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);

            MailComponentS mailComponentS = await scene.GetComponent<DBManagerComponent>().GetZoneDB(scene.Zone()).Query<MailComponentS>(request.UnitId);

            if (mailComponentS == null)
            {
                mailUnit.AddComponent<MailComponentS>();
            }
            else
            {
                mailUnit.AddComponent(mailComponentS);
            }

            await mailUnit.AddLocation(LocationType.Mail);

            await ETTask.CompletedTask;
        }
    }
}