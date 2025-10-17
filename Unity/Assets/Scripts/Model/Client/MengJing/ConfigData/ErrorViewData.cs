using System.Collections.Generic;

namespace ET.Client
{
    public static class ErrorViewData
    {
        [StaticField]
        public static Dictionary<int, string> ErrorHints = new()
        {
            { ErrorCode.ERR_NetWorkError, "网络错误!" },
            { ErrorCode.ERR_AccountAlreadyRegister, "账号已注册!" },
            { ErrorCode.ERR_AccountInBlackListError, "账号异常!" },
            { ErrorCode.ERR_LoginInfoIsNull, "未找到账号数据，请确认账号是否已经注册。" },
            { ErrorCode.ERR_AccountOrPasswordError, "密码错误，请检查重新输入。" },
            { ErrorCode.ERR_OtherAccountLogin, "账号异地登录" },
            { ErrorCode.ERR_RequestRepeatedly, "请求重复" },
            { ErrorCode.ERR_EnterQueue, "服务器已满，进入排队系统。" },
            { ErrorCode.ERR_RequestExitFuben, "请先退出副本" },
            { ErrorCode.ERR_StopServer, "停服维护" },
            { ErrorCode.ERR_BingPhoneError_1, "手机号已经注册过账号" },
            { ErrorCode.ERR_BingPhoneError_2, "手机号只能绑定一个账号" },
            { ErrorCode.ERR_VersionNoMatch, "版本不一致，请重开客户端。" },
            { ErrorCode.ERR_EnterGameError, "角色登录异常，请尝试再次重新登录账号!" },

        };
    }
}
