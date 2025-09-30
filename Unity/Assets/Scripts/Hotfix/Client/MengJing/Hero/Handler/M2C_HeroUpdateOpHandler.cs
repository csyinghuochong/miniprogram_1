namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_HeroUpdateOpHandler : MessageHandler<Scene, M2C_HeroUpdateOp>
    {
        protected override async ETTask Run(Scene root, M2C_HeroUpdateOp message)
        {
            HeroComponentC heroComponentC = root.GetComponent<HeroComponentC>();

            if (message.HeroOpType == (int)HeroOpType.Add)
            {
                heroComponentC.AddHeroFromMessage(message.HeroInfo);
            }
            else if (message.HeroOpType == (int)HeroOpType.Remove)
            {
                heroComponentC.RemoveHeroById(message.HeroInfo.Id);
            }
            else if (message.HeroOpType == (int)HeroOpType.Update)
            {
                heroComponentC.UpdateHero(message.HeroInfo);
            }

            await ETTask.CompletedTask;
        }
    }
}