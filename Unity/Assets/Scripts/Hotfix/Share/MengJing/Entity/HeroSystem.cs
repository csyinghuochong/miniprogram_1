namespace ET
{
    [EntitySystemOf(typeof(Hero))]
    [FriendOf(typeof(Hero))]
    public static partial class HeroSystem
    {
        [EntitySystem]
        private static void Awake(this Hero self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Hero self)
        {
        }

        public static HeroInfo ToMessage(this Hero self)
        {
            HeroInfo heroInfo = HeroInfo.Create();
            heroInfo.Id = self.Id;
            heroInfo.ConfigId = self.ConfigId;
            heroInfo.Lv = self.Lv;
            heroInfo.Exp = self.Exp;
            heroInfo.Star = self.Star;

            return heroInfo;
        }

        public static void FromMessage(this Hero self, HeroInfo heroInfo)
        {
            self.ConfigId = heroInfo.ConfigId;
            self.Lv = heroInfo.Lv;
            self.Exp = heroInfo.Exp;
            self.Star = heroInfo.Star;
        }
    }
}