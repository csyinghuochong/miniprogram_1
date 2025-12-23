namespace ET
{
    public enum ChatChannelType
    {
        World = 0, //世界 
        LianMeng = 1, //联盟
    }

    public static class NoticeType
    {
        public const int Notice = 0;
        public const int StopSever = 1; //停服
        public const int RemoteLogin = 2; //异地登录

        public const int UnitDisconnect = 3;
        public const int BattleOpen = 3;
        public const int BattleNotice = 4;
        public const int BattleOver = 5;
        public const int TeamDungeon = 6;
        public const int YeWaiBoss = 7;
        public const int PlayerExit = 8;
        public const int ArenaOpen = 9;
        public const int ArenaClose = 10;
        public const int TianQiChange = 11;
        public const int PaiMaiAuction = 12;
        public const int RankRefresh = 13;
        public const int UnionRace = 14;
        public const int SoloBegin = 15;
        public const int SoloOver = 16;
        public const int KillEvent = 17;
        public const int CreateRobot = 18;
        public const int BaoZang = 19;
        public const int RunRace = 20;
        public const int Demon = 21;
        public const int PaiMai = 22;
        public const int DragonDungeon = 23;
        public const int PetMatchOpen = 24;
        public const int PetMatchClose = 25;
    }
}