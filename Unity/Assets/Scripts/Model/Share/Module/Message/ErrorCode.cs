namespace ET
{
    public static partial class ErrorCode
    {
        // 成功
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
        
        // 通用错误
        public const int ERR_Error = 200002; //通用错误
        public const int ERR_NetWorkError = 200003; //网络错误
        public const int ERR_OperationOften = 200004; //操作太频繁

        // 账号相关错误 200101-200199
        public const int ERR_AccountAlreadyRegister = 200101; //表示账号已经被注册
        public const int ERR_AccountOrPasswordError = 200102; //登录时,表示密码错误
        public const int ERR_RequestRepeatedly = 200103; //请求重复
        public const int ERR_LoginInfoIsNull = 200104; //登录出错
        public const int ERR_AccountInBlackListError = 200105; //账号黑名单
        public const int ERR_OtherAccountLogin = 200106; //异地登录
        public const int ERR_TokenError = 200107; //token错误
        public const int ERR_NotFindAccount = 20132; //账号不存在
        public const int ERR_AccountNameFormError = 200129;
        public const int ERR_PasswordFormError = 20130;

        // 登录相关错误 200108-200129
        public const int ERR_EnterGameError = 200108;
        public const int ERR_ReEnterGameError = 200109;
        public const int ERR_ReEnterGameError2 = 200109; // 注意：与ERR_ReEnterGameError相同
        public const int ERR_SessionPlayerError = 200110;
        public const int ERR_NonePlayerError = 200111;
        public const int ERR_PlayerSessionError = 200112;
        public const int ERR_SessionDisconnect = 200113;
        public const int ERR_LoginTimeOut = 200114; //登录超时
        public const int ERR_EnterQueue = 200115; //进入排队    
        public const int ERR_LoginRealm = 200116;
        public const int ERR_StopServer = 200117;
        public const int ERR_LoginGameGateError01 = 200128;

        // 手机绑定错误 200118-200119
        public const int ERR_BingPhoneError_1 = 200118;
        public const int ERR_BingPhoneError_2 = 200119;

        // 版本和数据错误 200120-200127
        public const int ERR_VersionNoMatch = 200120;
        public const int ERR_ModifyData = 200121;
        public const int ERR_PaiMaiBuyMaxPage = 200122; //拍卖达到最大页数
        public const int Pre_Condition_Error = 200123; //前置条件不足
        public const int ERR_RequestExitFuben = 200124;
        public const int ERR_KickOutPlayer = 200125; //长时间不操作被踢下线
        public const int ERR_PackageFrequent = 200126; //发送太频繁
        public const int ERR_AlreadyHave = 200127;

        // 道具相关错误 20133-20138
        public const int ERR_CreateRoleName = 20133; //角色名字不合法
        public const int ERR_NotEnoughItems = 20134; //道具不足
        public const int ERR_NotExistItem = 20135; //道具不存在
        public const int ERR_InventoryContainerError = 20137; //背包容器错误
        public const int ERR_ItemUseNumError = 20138; //道具使用数量错误

        // 英雄相关错误 20136, 20139
        public const int ERR_HeroNotEquipSlot = 20136; //英雄没有这个类型的装备孔位
        public const int ERR_NotExistHero = 20139; //英雄不存在

        // 冒险/关卡相关错误 20140-20143
        public const int ERR_AlreadyAdventureState = 20140; //已在闯关中
        public const int ERR_AdventureLevelIdError = 20141;
        public const int ERR_AdventureWinResultError = 20142; //战斗胜利数据异常
        public const int ERR_LevelIsNot = 20143; //关卡Id不存在

        // 技能相关错误 20131, 20145-20146
        public const int ERR_UseSkillInCD = 20131;
        public const int ERR_UseSkillInPublicCD = 20146; //技能公共CD
        public const int ERR_NotSkillHandler = 20145; //技能没有配置SkillHandler
        public const int ERR_TargetUnitIsNull = 20147; //目标单位不存在
        public const int ERR_TargetUnitCantBeAttack = 20148; //目标被不能攻击

        // 场景相关错误 20144
        public const int ERR_SceneCantSetTimeScale = 20144; //当前Scene不能设置TimeScale

        // 任务相关错误 20147-20148
        public const int ERR_TaskCommited = 20147; //任务已经提交
        public const int ERR_TaskNoCompleted = 20148; //任务没有完成
    }
}