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
        
        // 通用错误 200001-200099
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
        public const int ERR_NotFindAccount = 200108; //账号不存在
        public const int ERR_AccountNameFormError = 200109; //账户名格式错误
        public const int ERR_PasswordFormError = 200110; //密码格式错误

        // 登录相关错误 200201-200299
        public const int ERR_EnterGameError = 200201; //进入游戏错误
        public const int ERR_ReEnterGameError = 200202; //重复进入游戏错误
        public const int ERR_SessionPlayerError = 200203; //会话玩家错误
        public const int ERR_NonePlayerError = 200204; //无玩家错误
        public const int ERR_PlayerSessionError = 200205; //玩家会话错误
        public const int ERR_SessionDisconnect = 200206; //会话断开连接
        public const int ERR_LoginTimeOut = 200207; //登录超时
        public const int ERR_EnterQueue = 200208; //进入排队
        public const int ERR_LoginRealm = 200209; //登录领域服务器错误
        public const int ERR_StopServer = 200210; //服务器停止服务
        public const int ERR_LoginGameGateError01 = 200211; //网关登录错误

        // 手机绑定错误 200301-200399
        public const int ERR_BingPhoneError_1 = 200301; //手机绑定错误1
        public const int ERR_BingPhoneError_2 = 200302; //手机绑定错误2

        // 版本和数据错误 200401-200499
        public const int ERR_VersionNoMatch = 200401; //版本不匹配
        public const int ERR_ModifyData = 200402; //数据修改错误
        public const int ERR_PaiMaiBuyMaxPage = 200403; //拍卖达到最大页数
        public const int Pre_Condition_Error = 200404; //前置条件不足
        public const int ERR_RequestExitFuben = 200405; //请求退出副本
        public const int ERR_KickOutPlayer = 200406; //长时间不操作被踢下线
        public const int ERR_PackageFrequent = 200407; //发送包过于频繁
        public const int ERR_AlreadyHave = 200408; //已经拥有

        // 角色相关错误 200501-200599
        public const int ERR_CreateRoleName = 200501; //角色名字不合法

        // 道具相关错误 200601-200699
        public const int ERR_NotEnoughItems = 200601; //道具不足
        public const int ERR_NotExistItem = 200602; //道具不存在
        public const int ERR_InventoryContainerError = 200603; //背包容器错误
        public const int ERR_ItemUseNumError = 200604; //道具使用数量错误

        // 英雄相关错误 200701-200799
        public const int ERR_HeroNotEquipSlot = 200701; //英雄没有这个类型的装备孔位
        public const int ERR_NotExistHero = 200702; //英雄不存在

        // 冒险/关卡相关错误 200801-200899
        public const int ERR_AlreadyAdventureState = 200801; //已在闯关中
        public const int ERR_AdventureLevelIdError = 200802; //冒险关卡ID错误
        public const int ERR_AdventureWinResultError = 200803; //战斗胜利数据异常
        public const int ERR_LevelIsNot = 200804; //关卡Id不存在

        // 技能相关错误 200901-200999
        public const int ERR_UseSkillInCD = 200901; //技能在冷却中
        public const int ERR_UseSkillInPublicCD = 200902; //技能公共冷却时间
        public const int ERR_NotSkillHandler = 200903; //技能没有配置SkillHandler
        public const int ERR_TargetUnitIsNull = 200904; //目标单位不存在
        public const int ERR_TargetUnitCantBeAttack = 200905; //目标不能被攻击
        public const int ERR_Stun = 200906; //眩晕状态中
        public const int ERR_Freeze= 200907; //冰冻状态中

        // 场景相关错误 201001-201099
        public const int ERR_SceneCantSetTimeScale = 201001; //当前Scene不能设置TimeScale

        // 任务相关错误 201101-201199
        public const int ERR_TaskCommited = 201101; //任务已经提交
        public const int ERR_TaskNoCompleted = 201102; //任务没有完成
        
        public const int ERR_MailNotExist = 201201; //邮件不存在
        public const int ERR_MailDeleted = 201202; //邮件已删除
        public const int ERR_MailRewardAlreadyReceived = 201203; //邮件道具已领取
        public const int ERR_MailNotReward = 201204; //邮件没有奖励
        public const int ERR_MailTimeOut = 201205; //邮件过期
        
        public const int ERR_StoreItemNotExist = 201301; //商店道具不存在
        public const int ERR_StoreItemNotEnough = 201302; //商店道具不足
        public const int ERR_StoreRefreshNumNotEnough = 201303; //商店刷新次数不足
    }
}