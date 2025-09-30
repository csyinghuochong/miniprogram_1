namespace ET.Server
{
    public static class HeroNoticeHelper
    {
        public static void SyncHeroInfo(Unit unit, Hero hero, HeroOpType heroOpType)
        {
            M2C_HeroUpdateOp m2CHeroUpdateOp = M2C_HeroUpdateOp.Create();
            m2CHeroUpdateOp.HeroInfo = hero.ToMessage();
            m2CHeroUpdateOp.HeroOpType = (int)heroOpType;
            MapMessageHelper.SendToClient(unit, m2CHeroUpdateOp);
        }
    }
}