using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(OuterMessage.HttpGetRouterResponse)]
    public partial class HttpGetRouterResponse : MessageObject
    {
        public static HttpGetRouterResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(HttpGetRouterResponse), isFromPool) as HttpGetRouterResponse;
        }

        [MemoryPackOrder(0)]
        public List<string> Realms { get; set; } = new();

        [MemoryPackOrder(1)]
        public List<string> Routers { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Realms.Clear();
            this.Routers.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.RouterSync)]
    public partial class RouterSync : MessageObject
    {
        public static RouterSync Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(RouterSync), isFromPool) as RouterSync;
        }

        [MemoryPackOrder(0)]
        public uint ConnectId { get; set; }

        [MemoryPackOrder(1)]
        public string Address { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ConnectId = default;
            this.Address = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_TestRequest)]
    [ResponseType(nameof(M2C_TestResponse))]
    public partial class C2M_TestRequest : MessageObject, ILocationRequest
    {
        public static C2M_TestRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_TestRequest), isFromPool) as C2M_TestRequest;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string request { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.request = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_TestResponse)]
    public partial class M2C_TestResponse : MessageObject, IResponse
    {
        public static M2C_TestResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_TestResponse), isFromPool) as M2C_TestResponse;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public string response { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.response = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2G_EnterGame)]
    [ResponseType(nameof(G2C_EnterGame))]
    public partial class C2G_EnterGame : MessageObject, ISessionRequest
    {
        public static C2G_EnterGame Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_EnterGame), isFromPool) as C2G_EnterGame;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long UnitId { get; set; }

        [MemoryPackOrder(2)]
        public long AccountId { get; set; }

        [MemoryPackOrder(3)]
        public int ReLink { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;
            this.AccountId = default;
            this.ReLink = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.G2C_EnterGame)]
    public partial class G2C_EnterGame : MessageObject, ISessionResponse
    {
        public static G2C_EnterGame Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_EnterGame), isFromPool) as G2C_EnterGame;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        /// <summary>
        /// 自己unitId
        /// </summary>
        [MemoryPackOrder(3)]
        public long MyId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.MyId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.MoveInfo)]
    public partial class MoveInfo : MessageObject
    {
        public static MoveInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(MoveInfo), isFromPool) as MoveInfo;
        }

        [MemoryPackOrder(0)]
        public List<Unity.Mathematics.float3> Points { get; set; } = new();

        [MemoryPackOrder(1)]
        public Unity.Mathematics.quaternion Rotation { get; set; }

        [MemoryPackOrder(2)]
        public int TurnSpeed { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Points.Clear();
            this.Rotation = default;
            this.TurnSpeed = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.UnitInfo)]
    public partial class UnitInfo : MessageObject
    {
        public static UnitInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(UnitInfo), isFromPool) as UnitInfo;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public int ConfigId { get; set; }

        [MemoryPackOrder(2)]
        public int Type { get; set; }

        [MemoryPackOrder(3)]
        public Unity.Mathematics.float3 Position { get; set; }

        [MemoryPackOrder(4)]
        public Unity.Mathematics.float3 Forward { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(5)]
        public Dictionary<int, long> KV { get; set; } = new();
        [MemoryPackOrder(6)]
        public MoveInfo MoveInfo { get; set; }

        [MemoryPackOrder(20)]
        public string UnitName { get; set; }

        [MemoryPackOrder(21)]
        public string MasterName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.ConfigId = default;
            this.Type = default;
            this.Position = default;
            this.Forward = default;
            this.KV.Clear();
            this.MoveInfo = default;
            this.UnitName = default;
            this.MasterName = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_CreateUnits)]
    public partial class M2C_CreateUnits : MessageObject, IMessage
    {
        public static M2C_CreateUnits Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_CreateUnits), isFromPool) as M2C_CreateUnits;
        }

        [MemoryPackOrder(0)]
        public List<UnitInfo> Units { get; set; } = new();

        [MemoryPackOrder(7)]
        public int UpdateAll { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Units.Clear();
            this.UpdateAll = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_CreateMyUnit)]
    public partial class M2C_CreateMyUnit : MessageObject, IMessage
    {
        public static M2C_CreateMyUnit Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_CreateMyUnit), isFromPool) as M2C_CreateMyUnit;
        }

        [MemoryPackOrder(0)]
        public UnitInfo Unit { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Unit = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_StartSceneChange)]
    public partial class M2C_StartSceneChange : MessageObject, IMessage
    {
        public static M2C_StartSceneChange Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_StartSceneChange), isFromPool) as M2C_StartSceneChange;
        }

        [MemoryPackOrder(0)]
        public long SceneInstanceId { get; set; }

        [MemoryPackOrder(1)]
        public int MapType { get; set; }

        [MemoryPackOrder(2)]
        public int SceneId { get; set; }

        [MemoryPackOrder(3)]
        public float TimeScale { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.SceneInstanceId = default;
            this.MapType = default;
            this.SceneId = default;
            this.TimeScale = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_RemoveUnits)]
    public partial class M2C_RemoveUnits : MessageObject, IMessage
    {
        public static M2C_RemoveUnits Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_RemoveUnits), isFromPool) as M2C_RemoveUnits;
        }

        [MemoryPackOrder(0)]
        public List<long> Units { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Units.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_PathfindingRequest)]
    public partial class C2M_PathfindingRequest : MessageObject, ILocationMessage
    {
        public static C2M_PathfindingRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_PathfindingRequest), isFromPool) as C2M_PathfindingRequest;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public Unity.Mathematics.float3 Position { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Position = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 客户端寻路
    /// </summary>
    [MemoryPackable]
    [Message(OuterMessage.C2M_PathfindingResult)]
    public partial class C2M_PathfindingResult : MessageObject, ILocationMessage
    {
        public static C2M_PathfindingResult Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_PathfindingResult), isFromPool) as C2M_PathfindingResult;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(2)]
        public int SpeedRate { get; set; }

        /// <summary>
        /// 服务器时间戳
        /// </summary>
        [MemoryPackOrder(3)]
        public long ServerTime { get; set; }

        [MemoryPackOrder(4)]
        public List<Unity.Mathematics.float3> Position { get; set; } = new();

        /// <summary>
        /// 当前位置
        /// </summary>
        [MemoryPackOrder(5)]
        public Unity.Mathematics.float3 Current { get; set; }

        /// <summary>
        /// 当前帧
        /// </summary>
        [MemoryPackOrder(6)]
        public int Frame { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.SpeedRate = default;
            this.ServerTime = default;
            this.Position.Clear();
            this.Current = default;
            this.Frame = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_Stop)]
    public partial class C2M_Stop : MessageObject, ILocationMessage
    {
        public static C2M_Stop Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_Stop), isFromPool) as C2M_Stop;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(3)]
        public bool YaoGan { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.YaoGan = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_Stop)]
    public partial class M2C_Stop : MessageObject, IMessage
    {
        public static M2C_Stop Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_Stop), isFromPool) as M2C_Stop;
        }

        [MemoryPackOrder(0)]
        public int Error { get; set; }

        [MemoryPackOrder(1)]
        public long Id { get; set; }

        [MemoryPackOrder(2)]
        public Unity.Mathematics.float3 Position { get; set; }

        [MemoryPackOrder(3)]
        public Unity.Mathematics.quaternion Rotation { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Error = default;
            this.Id = default;
            this.Position = default;
            this.Rotation = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_StopResult)]
    public partial class C2M_StopResult : MessageObject, ILocationMessage
    {
        public static C2M_StopResult Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_StopResult), isFromPool) as C2M_StopResult;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(2)]
        public Unity.Mathematics.float3 Position { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Position = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_StopResult)]
    public partial class M2C_StopResult : MessageObject, IMessage
    {
        public static M2C_StopResult Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_StopResult), isFromPool) as M2C_StopResult;
        }

        [MemoryPackOrder(0)]
        public int Error { get; set; }

        [MemoryPackOrder(1)]
        public long Id { get; set; }

        [MemoryPackOrder(2)]
        public Unity.Mathematics.float3 Position { get; set; }

        [MemoryPackOrder(3)]
        public Unity.Mathematics.quaternion Rotation { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Error = default;
            this.Id = default;
            this.Position = default;
            this.Rotation = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_PathfindingResult)]
    public partial class M2C_PathfindingResult : MessageObject, IMessage
    {
        public static M2C_PathfindingResult Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_PathfindingResult), isFromPool) as M2C_PathfindingResult;
        }

        [MemoryPackOrder(0)]
        public long Id { get; set; }

        [MemoryPackOrder(1)]
        public Unity.Mathematics.float3 Position { get; set; }

        [MemoryPackOrder(2)]
        public List<Unity.Mathematics.float3> Points { get; set; } = new();

        [MemoryPackOrder(3)]
        public bool YaoGan { get; set; }

        [MemoryPackOrder(4)]
        public int SpeedRate { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Id = default;
            this.Position = default;
            this.Points.Clear();
            this.YaoGan = default;
            this.SpeedRate = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2G_Ping)]
    [ResponseType(nameof(G2C_Ping))]
    public partial class C2G_Ping : MessageObject, ISessionRequest
    {
        public static C2G_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_Ping), isFromPool) as C2G_Ping;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.G2C_Ping)]
    public partial class G2C_Ping : MessageObject, ISessionResponse
    {
        public static G2C_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Ping), isFromPool) as G2C_Ping;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long Time { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Time = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_Ping)]
    [ResponseType(nameof(R2C_Ping))]
    public partial class C2R_Ping : MessageObject, ISessionRequest
    {
        public static C2R_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_Ping), isFromPool) as C2R_Ping;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_Ping)]
    public partial class R2C_Ping : MessageObject, ISessionResponse
    {
        public static R2C_Ping Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_Ping), isFromPool) as R2C_Ping;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long Time { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Time = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.G2C_Test)]
    public partial class G2C_Test : MessageObject, ISessionMessage
    {
        public static G2C_Test Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Test), isFromPool) as G2C_Test;
        }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            
            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_Reload)]
    [ResponseType(nameof(M2C_Reload))]
    public partial class C2M_Reload : MessageObject, ISessionRequest
    {
        public static C2M_Reload Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_Reload), isFromPool) as C2M_Reload;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Account { get; set; }

        [MemoryPackOrder(2)]
        public string Password { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Account = default;
            this.Password = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_Reload)]
    public partial class M2C_Reload : MessageObject, ISessionResponse
    {
        public static M2C_Reload Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_Reload), isFromPool) as M2C_Reload;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.ServerItem)]
    public partial class ServerItem : MessageObject
    {
        public static ServerItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(ServerItem), isFromPool) as ServerItem;
        }

        [MemoryPackOrder(0)]
        public int ServerId { get; set; }

        [MemoryPackOrder(1)]
        public string ServerIp { get; set; }

        [MemoryPackOrder(2)]
        public string ServerName { get; set; }

        [MemoryPackOrder(3)]
        public long ServerOpenTime { get; set; }

        [MemoryPackOrder(4)]
        public int Show { get; set; }

        [MemoryPackOrder(5)]
        public int New { get; set; }

        /// <summary>
        /// 不配置或者-1全部显示
        /// </summary>
        [MemoryPackOrder(6)]
        public List<int> PlatformList { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        [MemoryPackOrder(7)]
        public List<long> OldServerIds { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ServerId = default;
            this.ServerIp = default;
            this.ServerName = default;
            this.ServerOpenTime = default;
            this.Show = default;
            this.New = default;
            this.PlatformList.Clear();
            this.OldServerIds.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_DeleteAccountRequest)]
    [ResponseType(nameof(R2C_DeleteAccountResponse))]
    public partial class C2R_DeleteAccountRequest : MessageObject, ISessionRequest
    {
        public static C2R_DeleteAccountRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_DeleteAccountRequest), isFromPool) as C2R_DeleteAccountRequest;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(0)]
        public string Account { get; set; }

        [MemoryPackOrder(1)]
        public string Password { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Account = default;
            this.Password = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_DeleteAccountResponse)]
    public partial class R2C_DeleteAccountResponse : MessageObject, ISessionResponse
    {
        public static R2C_DeleteAccountResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_DeleteAccountResponse), isFromPool) as R2C_DeleteAccountResponse;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_ServerList)]
    [ResponseType(nameof(R2C_ServerList))]
    public partial class C2R_ServerList : MessageObject, ISessionRequest
    {
        public static C2R_ServerList Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_ServerList), isFromPool) as C2R_ServerList;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int VersionMode { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.VersionMode = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_ServerList)]
    public partial class R2C_ServerList : MessageObject, ISessionResponse
    {
        public static R2C_ServerList Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_ServerList), isFromPool) as R2C_ServerList;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Message { get; set; }

        [MemoryPackOrder(2)]
        public int Error { get; set; }

        /// <summary>
        /// 服务器列表
        /// </summary>
        [MemoryPackOrder(3)]
        public List<ServerItem> ServerItems { get; set; } = new();

        [MemoryPackOrder(4)]
        public string NoticeVersion { get; set; }

        [MemoryPackOrder(5)]
        public string NoticeText { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Message = default;
            this.Error = default;
            this.ServerItems.Clear();
            this.NoticeVersion = default;
            this.NoticeText = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_LoginAccount)]
    [ResponseType(nameof(R2C_LoginAccount))]
    public partial class C2R_LoginAccount : MessageObject, ISessionRequest
    {
        public static C2R_LoginAccount Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_LoginAccount), isFromPool) as C2R_LoginAccount;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        [MemoryPackOrder(1)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MemoryPackOrder(2)]
        public string Password { get; set; }

        [MemoryPackOrder(3)]
        public string Token { get; set; }

        [MemoryPackOrder(4)]
        public string ThirdLogin { get; set; }

        [MemoryPackOrder(5)]
        public int Relink { get; set; }

        [MemoryPackOrder(6)]
        public int age_type { get; set; }

        [MemoryPackOrder(7)]
        public int ServerId { get; set; }

        [MemoryPackOrder(8)]
        public bool CheckRealName { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Account = default;
            this.Password = default;
            this.Token = default;
            this.ThirdLogin = default;
            this.Relink = default;
            this.age_type = default;
            this.ServerId = default;
            this.CheckRealName = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_LoginAccount)]
    public partial class R2C_LoginAccount : MessageObject, ISessionResponse
    {
        public static R2C_LoginAccount Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_LoginAccount), isFromPool) as R2C_LoginAccount;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public string Address { get; set; }

        [MemoryPackOrder(5)]
        public long GateId { get; set; }

        [MemoryPackOrder(6)]
        public string Token { get; set; }

        [MemoryPackOrder(7)]
        public long AccountId { get; set; }

        [MemoryPackOrder(8)]
        public int QueueNumber { get; set; }

        [MemoryPackOrder(9)]
        public string QueueAddres { get; set; }

        [MemoryPackOrder(10)]
        public PlayerInfo PlayerInfo { get; set; }

        [MemoryPackOrder(11)]
        public List<CreateRoleInfo> RoleLists { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Address = default;
            this.GateId = default;
            this.Token = default;
            this.AccountId = default;
            this.QueueNumber = default;
            this.QueueAddres = default;
            this.PlayerInfo = default;
            this.RoleLists.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_RealNameRequest)]
    [ResponseType(nameof(R2C_RealNameResponse))]
    public partial class C2R_RealNameRequest : MessageObject, ISessionRequest
    {
        public static C2R_RealNameRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_RealNameRequest), isFromPool) as C2R_RealNameRequest;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MemoryPackOrder(0)]
        public string Name { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [MemoryPackOrder(1)]
        public string IdCardNO { get; set; }

        /// <summary>
        /// 1认证 2查询 3上报
        /// </summary>
        [MemoryPackOrder(2)]
        public int AiType { get; set; }

        /// <summary>
        /// 帐号Id
        /// </summary>
        [MemoryPackOrder(3)]
        public long AccountId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Name = default;
            this.IdCardNO = default;
            this.AiType = default;
            this.AccountId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_RealNameResponse)]
    public partial class R2C_RealNameResponse : MessageObject, ISessionResponse
    {
        public static R2C_RealNameResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_RealNameResponse), isFromPool) as R2C_RealNameResponse;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        /// <summary>
        /// 实名认证返回
        /// </summary>
        [MemoryPackOrder(0)]
        public int ErrorCode { get; set; }

        [MemoryPackOrder(10)]
        public PlayerInfo PlayerInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ErrorCode = default;
            this.PlayerInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_RealNameRequest)]
    [ResponseType(nameof(M2C_RealNameResponse))]
    public partial class C2M_RealNameRequest : MessageObject, ILocationRequest
    {
        public static C2M_RealNameRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_RealNameRequest), isFromPool) as C2M_RealNameRequest;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MemoryPackOrder(0)]
        public string Name { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [MemoryPackOrder(1)]
        public string IdCardNO { get; set; }

        /// <summary>
        /// 1认证 2查询 3上报
        /// </summary>
        [MemoryPackOrder(2)]
        public int AiType { get; set; }

        /// <summary>
        /// 帐号Id
        /// </summary>
        [MemoryPackOrder(3)]
        public long AccountId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Name = default;
            this.IdCardNO = default;
            this.AiType = default;
            this.AccountId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_RealNameResponse)]
    public partial class M2C_RealNameResponse : MessageObject, ILocationResponse
    {
        public static M2C_RealNameResponse Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_RealNameResponse), isFromPool) as M2C_RealNameResponse;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        /// <summary>
        /// 实名认证返回
        /// </summary>
        [MemoryPackOrder(0)]
        public int ErrorCode { get; set; }

        [MemoryPackOrder(10)]
        public PlayerInfo PlayerInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ErrorCode = default;
            this.PlayerInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.RechargeInfo)]
    public partial class RechargeInfo : MessageObject
    {
        public static RechargeInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(RechargeInfo), isFromPool) as RechargeInfo;
        }

        [MemoryPackOrder(0)]
        public int Amount { get; set; }

        [MemoryPackOrder(1)]
        public long Time { get; set; }

        [MemoryPackOrder(2)]
        public long UnitId { get; set; }

        [MemoryPackOrder(3)]
        public string OrderInfo { get; set; }

        [MemoryPackOrder(4)]
        public int PhysicsZone { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Amount = default;
            this.Time = default;
            this.UnitId = default;
            this.OrderInfo = default;
            this.PhysicsZone = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.PlayerInfo)]
    public partial class PlayerInfo : MessageObject
    {
        public static PlayerInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(PlayerInfo), isFromPool) as PlayerInfo;
        }

        [MemoryPackOrder(0)]
        public int RealName { get; set; }

        [MemoryPackOrder(1)]
        public string Name { get; set; }

        [MemoryPackOrder(2)]
        public string IdCardNo { get; set; }

        [MemoryPackOrder(3)]
        public int RealNameReward { get; set; }

        [MemoryPackOrder(4)]
        public List<RechargeInfo> RechargeInfos { get; set; } = new();

        [MemoryPackOrder(5)]
        public List<KeyValuePair> DeleteUserList { get; set; } = new();

        [MemoryPackOrder(6)]
        public List<int> BuChangZone { get; set; } = new();

        [MemoryPackOrder(7)]
        public string PhoneNumber { get; set; }

        [MemoryPackOrder(8)]
        public List<long> ShareTimes { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RealName = default;
            this.Name = default;
            this.IdCardNo = default;
            this.RealNameReward = default;
            this.RechargeInfos.Clear();
            this.DeleteUserList.Clear();
            this.BuChangZone.Clear();
            this.PhoneNumber = default;
            this.ShareTimes.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.CreateRoleInfo)]
    public partial class CreateRoleInfo : MessageObject
    {
        public static CreateRoleInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(CreateRoleInfo), isFromPool) as CreateRoleInfo;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public int PlayerLv { get; set; }

        [MemoryPackOrder(2)]
        public int PlayerOcc { get; set; }

        [MemoryPackOrder(3)]
        public string PlayerName { get; set; }

        [MemoryPackOrder(4)]
        public int RobotId { get; set; }

        [MemoryPackOrder(5)]
        public int ServerId { get; set; }

        [MemoryPackOrder(6)]
        public int State { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.PlayerLv = default;
            this.PlayerOcc = default;
            this.PlayerName = default;
            this.RobotId = default;
            this.ServerId = default;
            this.State = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2G_LoginGameGate)]
    [ResponseType(nameof(G2C_LoginGameGate))]
    public partial class C2G_LoginGameGate : MessageObject, ISessionRequest
    {
        public static C2G_LoginGameGate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_LoginGameGate), isFromPool) as C2G_LoginGameGate;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long Key { get; set; }

        [MemoryPackOrder(2)]
        public long GateId { get; set; }

        [MemoryPackOrder(3)]
        public string AccountName { get; set; }

        [MemoryPackOrder(4)]
        public long RoleId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Key = default;
            this.GateId = default;
            this.AccountName = default;
            this.RoleId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.G2C_LoginGameGate)]
    public partial class G2C_LoginGameGate : MessageObject, ISessionResponse
    {
        public static G2C_LoginGameGate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_LoginGameGate), isFromPool) as G2C_LoginGameGate;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long PlayerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.PlayerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_CreateRoleData)]
    [ResponseType(nameof(R2C_CreateRoleData))]
    public partial class C2R_CreateRoleData : MessageObject, ISessionRequest
    {
        public static C2R_CreateRoleData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_CreateRoleData), isFromPool) as C2R_CreateRoleData;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int CreateOcc { get; set; }

        [MemoryPackOrder(2)]
        public string CreateName { get; set; }

        [MemoryPackOrder(3)]
        public long AccountId { get; set; }

        [MemoryPackOrder(4)]
        public int ServerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.CreateOcc = default;
            this.CreateName = default;
            this.AccountId = default;
            this.ServerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_CreateRoleData)]
    public partial class R2C_CreateRoleData : MessageObject, ISessionResponse
    {
        public static R2C_CreateRoleData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_CreateRoleData), isFromPool) as R2C_CreateRoleData;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        [MemoryPackOrder(0)]
        public CreateRoleInfo createRoleInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.createRoleInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_DeleteRoleData)]
    [ResponseType(nameof(R2C_DeleteRoleData))]
    public partial class C2R_DeleteRoleData : MessageObject, ISessionRequest
    {
        public static C2R_DeleteRoleData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_DeleteRoleData), isFromPool) as C2R_DeleteRoleData;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(0)]
        public long AccountId { get; set; }

        [MemoryPackOrder(1)]
        public long UserId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.AccountId = default;
            this.UserId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_DeleteRoleData)]
    public partial class R2C_DeleteRoleData : MessageObject, ISessionResponse
    {
        public static R2C_DeleteRoleData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_DeleteRoleData), isFromPool) as R2C_DeleteRoleData;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Q_EnterQueue)]
    [ResponseType(nameof(Q2C_EnterQueue))]
    public partial class C2Q_EnterQueue : MessageObject, ISessionRequest
    {
        public static C2Q_EnterQueue Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Q_EnterQueue), isFromPool) as C2Q_EnterQueue;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(0)]
        public string Token { get; set; }

        [MemoryPackOrder(1)]
        public string Account { get; set; }

        [MemoryPackOrder(2)]
        public long AccountId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.Account = default;
            this.AccountId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Q2C_EnterQueue)]
    public partial class Q2C_EnterQueue : MessageObject, ISessionResponse
    {
        public static Q2C_EnterQueue Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Q2C_EnterQueue), isFromPool) as Q2C_EnterQueue;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2R_GetRealmKey)]
    [ResponseType(nameof(R2C_GetRealmKey))]
    public partial class C2R_GetRealmKey : MessageObject, ISessionRequest
    {
        public static C2R_GetRealmKey Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2R_GetRealmKey), isFromPool) as C2R_GetRealmKey;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string Token { get; set; }

        [MemoryPackOrder(2)]
        public string Account { get; set; }

        [MemoryPackOrder(3)]
        public int ServerId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Token = default;
            this.Account = default;
            this.ServerId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.R2C_GetRealmKey)]
    public partial class R2C_GetRealmKey : MessageObject, ISessionResponse
    {
        public static R2C_GetRealmKey Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(R2C_GetRealmKey), isFromPool) as R2C_GetRealmKey;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public string Address { get; set; }

        [MemoryPackOrder(4)]
        public long Key { get; set; }

        [MemoryPackOrder(5)]
        public long GateId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Address = default;
            this.Key = default;
            this.GateId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.G2C_TestHotfixMessage)]
    public partial class G2C_TestHotfixMessage : MessageObject, ISessionMessage
    {
        public static G2C_TestHotfixMessage Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_TestHotfixMessage), isFromPool) as G2C_TestHotfixMessage;
        }

        [MemoryPackOrder(0)]
        public string Info { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Info = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_TestRobotCase)]
    [ResponseType(nameof(M2C_TestRobotCase))]
    public partial class C2M_TestRobotCase : MessageObject, ILocationRequest
    {
        public static C2M_TestRobotCase Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_TestRobotCase), isFromPool) as C2M_TestRobotCase;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int N { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.N = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_TestRobotCase)]
    public partial class M2C_TestRobotCase : MessageObject, ILocationResponse
    {
        public static M2C_TestRobotCase Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_TestRobotCase), isFromPool) as M2C_TestRobotCase;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public int N { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.N = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_TestRobotCase2)]
    public partial class C2M_TestRobotCase2 : MessageObject, ILocationMessage
    {
        public static C2M_TestRobotCase2 Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_TestRobotCase2), isFromPool) as C2M_TestRobotCase2;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int N { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.N = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_TestRobotCase2)]
    public partial class M2C_TestRobotCase2 : MessageObject, ILocationMessage
    {
        public static M2C_TestRobotCase2 Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_TestRobotCase2), isFromPool) as M2C_TestRobotCase2;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int N { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.N = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_TransferMap)]
    [ResponseType(nameof(M2C_TransferMap))]
    public partial class C2M_TransferMap : MessageObject, ILocationRequest
    {
        public static C2M_TransferMap Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_TransferMap), isFromPool) as C2M_TransferMap;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int SceneId { get; set; }

        [MemoryPackOrder(2)]
        public int MapType { get; set; }

        [MemoryPackOrder(4)]
        public int Difficulty { get; set; }

        [MemoryPackOrder(5)]
        public string paramInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.SceneId = default;
            this.MapType = default;
            this.Difficulty = default;
            this.paramInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_TransferMap)]
    public partial class M2C_TransferMap : MessageObject, ILocationResponse
    {
        public static M2C_TransferMap Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_TransferMap), isFromPool) as M2C_TransferMap;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2G_Benchmark)]
    [ResponseType(nameof(G2C_Benchmark))]
    public partial class C2G_Benchmark : MessageObject, ISessionRequest
    {
        public static C2G_Benchmark Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_Benchmark), isFromPool) as C2G_Benchmark;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.G2C_Benchmark)]
    public partial class G2C_Benchmark : MessageObject, ISessionResponse
    {
        public static G2C_Benchmark Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Benchmark), isFromPool) as G2C_Benchmark;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.ServerInfo)]
    public partial class ServerInfo : MessageObject
    {
        public static ServerInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(ServerInfo), isFromPool) as ServerInfo;
        }

        [MemoryPackOrder(0)]
        public int WorldLv { get; set; }

        [MemoryPackOrder(1)]
        public long ExChangeGold { get; set; }

        [MemoryPackOrder(4)]
        public int TianQi { get; set; }

        [MemoryPackOrder(5)]
        public bool ShouLieOpen { get; set; }

        /// <summary>
        /// 每天随机
        /// </summary>
        [MemoryPackOrder(6)]
        public int ChouKaDropId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.WorldLv = default;
            this.ExChangeGold = default;
            this.TianQi = default;
            this.ShouLieOpen = default;
            this.ChouKaDropId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.A2C_Disconnect)]
    public partial class A2C_Disconnect : MessageObject, IMessage
    {
        public static A2C_Disconnect Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(A2C_Disconnect), isFromPool) as A2C_Disconnect;
        }

        [MemoryPackOrder(0)]
        public int Error { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Error = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.G2C_SecondLogin)]
    public partial class G2C_SecondLogin : MessageObject, IMessage
    {
        public static G2C_SecondLogin Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_SecondLogin), isFromPool) as G2C_SecondLogin;
        }

        [MemoryPackOrder(0)]
        public int Error { get; set; }

        [MemoryPackOrder(1)]
        public int SceneType { get; set; }

        [MemoryPackOrder(2)]
        public int SceneId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Error = default;
            this.SceneType = default;
            this.SceneId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UnitNumericListUpdate)]
    public partial class M2C_UnitNumericListUpdate : MessageObject, IMessage
    {
        public static M2C_UnitNumericListUpdate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UnitNumericListUpdate), isFromPool) as M2C_UnitNumericListUpdate;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(0)]
        public long UnitID { get; set; }

        [MemoryPackOrder(1)]
        public List<int> Ks { get; set; } = new();

        [MemoryPackOrder(2)]
        public List<long> Vs { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitID = default;
            this.Ks.Clear();
            this.Vs.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UnitNumericUpdate)]
    public partial class M2C_UnitNumericUpdate : MessageObject, IMessage
    {
        public static M2C_UnitNumericUpdate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UnitNumericUpdate), isFromPool) as M2C_UnitNumericUpdate;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(92)]
        public long ActorId { get; set; }

        [MemoryPackOrder(93)]
        public long UnitId { get; set; }

        [MemoryPackOrder(0)]
        public int SkillId { get; set; }

        [MemoryPackOrder(1)]
        public int NumericType { get; set; }

        [MemoryPackOrder(2)]
        public long OldValue { get; set; }

        [MemoryPackOrder(3)]
        public long NewValue { get; set; }

        [MemoryPackOrder(4)]
        public int DamgeType { get; set; }

        [MemoryPackOrder(5)]
        public long AttackId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ActorId = default;
            this.UnitId = default;
            this.SkillId = default;
            this.NumericType = default;
            this.OldValue = default;
            this.NewValue = default;
            this.DamgeType = default;
            this.AttackId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_RoleDataBroadcast)]
    public partial class M2C_RoleDataBroadcast : MessageObject, IMessage
    {
        public static M2C_RoleDataBroadcast Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_RoleDataBroadcast), isFromPool) as M2C_RoleDataBroadcast;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        /// <summary>
        /// UserDataType
        /// </summary>
        [MemoryPackOrder(0)]
        public int UpdateType { get; set; }

        [MemoryPackOrder(1)]
        public string UpdateTypeValue { get; set; }

        [MemoryPackOrder(2)]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.UpdateType = default;
            this.UpdateTypeValue = default;
            this.UnitId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_GMCommand)]
    public partial class C2M_GMCommand : MessageObject, ILocationMessage
    {
        public static C2M_GMCommand Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_GMCommand), isFromPool) as C2M_GMCommand;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string GMMsg { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.GMMsg = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.ItemInfo)]
    public partial class ItemInfo : MessageObject
    {
        public static ItemInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(ItemInfo), isFromPool) as ItemInfo;
        }

        [MemoryPackOrder(0)]
        public long Id { get; set; }

        [MemoryPackOrder(1)]
        public int ConfigId { get; set; }

        [MemoryPackOrder(2)]
        public int ContainerType { get; set; }

        [MemoryPackOrder(3)]
        public int Num { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Id = default;
            this.ConfigId = default;
            this.ContainerType = default;
            this.Num = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_GetAllItem)]
    [ResponseType(nameof(M2C_GetAllItem))]
    public partial class C2M_GetAllItem : MessageObject, ILocationRequest
    {
        public static C2M_GetAllItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_GetAllItem), isFromPool) as C2M_GetAllItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_GetAllItem)]
    public partial class M2C_GetAllItem : MessageObject, ILocationResponse
    {
        public static M2C_GetAllItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_GetAllItem), isFromPool) as M2C_GetAllItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ItemInfo> ItemList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ItemList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_ItemUpdateOp)]
    public partial class M2C_ItemUpdateOp : MessageObject, IMessage
    {
        public static M2C_ItemUpdateOp Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_ItemUpdateOp), isFromPool) as M2C_ItemUpdateOp;
        }

        [MemoryPackOrder(0)]
        public List<ItemInfo> ItemInfoRemoveList { get; set; } = new();

        [MemoryPackOrder(1)]
        public List<ItemInfo> ItemInfoUpdateList { get; set; } = new();

        [MemoryPackOrder(2)]
        public List<ItemInfo> ItemInfoAddList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ItemInfoRemoveList.Clear();
            this.ItemInfoUpdateList.Clear();
            this.ItemInfoAddList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_SellItem)]
    [ResponseType(nameof(M2C_SellItem))]
    public partial class C2M_SellItem : MessageObject, ILocationRequest
    {
        public static C2M_SellItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_SellItem), isFromPool) as C2M_SellItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long ItemId { get; set; }

        [MemoryPackOrder(2)]
        public int Num { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ItemId = default;
            this.Num = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_SellItem)]
    public partial class M2C_SellItem : MessageObject, ILocationResponse
    {
        public static M2C_SellItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_SellItem), isFromPool) as M2C_SellItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_UseItem)]
    [ResponseType(nameof(M2C_UseItem))]
    public partial class C2M_UseItem : MessageObject, ILocationRequest
    {
        public static C2M_UseItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_UseItem), isFromPool) as C2M_UseItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long ItemId { get; set; }

        [MemoryPackOrder(2)]
        public int Num { get; set; }

        [MemoryPackOrder(3)]
        public long HeroId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ItemId = default;
            this.Num = default;
            this.HeroId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UseItem)]
    public partial class M2C_UseItem : MessageObject, ILocationResponse
    {
        public static M2C_UseItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UseItem), isFromPool) as M2C_UseItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_MoveItem)]
    [ResponseType(nameof(M2C_MoveItem))]
    public partial class C2M_MoveItem : MessageObject, ILocationRequest
    {
        public static C2M_MoveItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_MoveItem), isFromPool) as C2M_MoveItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public List<long> ItemIdList { get; set; } = new();

        [MemoryPackOrder(2)]
        public int ContainerType { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ItemIdList.Clear();
            this.ContainerType = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_MoveItem)]
    public partial class M2C_MoveItem : MessageObject, ILocationResponse
    {
        public static M2C_MoveItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_MoveItem), isFromPool) as M2C_MoveItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_GetStoreInfo)]
    [ResponseType(nameof(M2C_GetStoreInfo))]
    public partial class C2M_GetStoreInfo : MessageObject, ILocationRequest
    {
        public static C2M_GetStoreInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_GetStoreInfo), isFromPool) as C2M_GetStoreInfo;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_GetStoreInfo)]
    public partial class M2C_GetStoreInfo : MessageObject, ILocationResponse
    {
        public static M2C_GetStoreInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_GetStoreInfo), isFromPool) as M2C_GetStoreInfo;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public long RefreshTime { get; set; }

        [MemoryPackOrder(4)]
        public int RefreshNum { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(5)]
        public Dictionary<int, int> StoreItemList { get; set; } = new();
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.RefreshTime = default;
            this.RefreshNum = default;
            this.StoreItemList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_StoreBuy)]
    [ResponseType(nameof(M2C_StoreBuy))]
    public partial class C2M_StoreBuy : MessageObject, ILocationRequest
    {
        public static C2M_StoreBuy Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_StoreBuy), isFromPool) as C2M_StoreBuy;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int StoreItemId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.StoreItemId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_StoreBuy)]
    public partial class M2C_StoreBuy : MessageObject, ILocationResponse
    {
        public static M2C_StoreBuy Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_StoreBuy), isFromPool) as M2C_StoreBuy;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_RefreshStore)]
    [ResponseType(nameof(M2C_RefreshStore))]
    public partial class C2M_RefreshStore : MessageObject, ILocationRequest
    {
        public static C2M_RefreshStore Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_RefreshStore), isFromPool) as C2M_RefreshStore;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_RefreshStore)]
    public partial class M2C_RefreshStore : MessageObject, ILocationResponse
    {
        public static M2C_RefreshStore Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_RefreshStore), isFromPool) as M2C_RefreshStore;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public int RefreshNum { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(4)]
        public Dictionary<int, int> StoreItemList { get; set; } = new();
        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.RefreshNum = default;
            this.StoreItemList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.HeroInfo)]
    public partial class HeroInfo : MessageObject
    {
        public static HeroInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(HeroInfo), isFromPool) as HeroInfo;
        }

        [MemoryPackOrder(0)]
        public long Id { get; set; }

        [MemoryPackOrder(1)]
        public int ConfigId { get; set; }

        [MemoryPackOrder(2)]
        public int Lv { get; set; }

        [MemoryPackOrder(3)]
        public int Exp { get; set; }

        [MemoryPackOrder(4)]
        public int Star { get; set; }

        [MemoryPackOrder(5)]
        public int HunShi { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(6)]
        public Dictionary<int, long> Equipments { get; set; } = new();
        [MongoDB.Bson.Serialization.Attributes.BsonDictionaryOptions(MongoDB.Bson.Serialization.Options.DictionaryRepresentation.ArrayOfArrays)]
        [MemoryPackOrder(7)]
        public Dictionary<int, long> NumericDic { get; set; } = new();
        [MemoryPackOrder(8)]
        public List<int> Skills { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Id = default;
            this.ConfigId = default;
            this.Lv = default;
            this.Exp = default;
            this.Star = default;
            this.HunShi = default;
            this.Equipments.Clear();
            this.NumericDic.Clear();
            this.Skills.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_GetAllHero)]
    [ResponseType(nameof(M2C_GetAllHero))]
    public partial class C2M_GetAllHero : MessageObject, ILocationRequest
    {
        public static C2M_GetAllHero Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_GetAllHero), isFromPool) as C2M_GetAllHero;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_GetAllHero)]
    public partial class M2C_GetAllHero : MessageObject, ILocationResponse
    {
        public static M2C_GetAllHero Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_GetAllHero), isFromPool) as M2C_GetAllHero;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<HeroInfo> HeroList { get; set; } = new();

        [MemoryPackOrder(4)]
        public List<long> Formation { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.HeroList.Clear();
            this.Formation.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_HeroUpdateOp)]
    public partial class M2C_HeroUpdateOp : MessageObject, IMessage
    {
        public static M2C_HeroUpdateOp Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_HeroUpdateOp), isFromPool) as M2C_HeroUpdateOp;
        }

        [MemoryPackOrder(0)]
        public HeroInfo HeroInfo { get; set; }

        [MemoryPackOrder(1)]
        public int HeroOpType { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.HeroInfo = default;
            this.HeroOpType = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_SetHeroFormation)]
    [ResponseType(nameof(M2C_SetHeroFormation))]
    public partial class C2M_SetHeroFormation : MessageObject, ILocationRequest
    {
        public static C2M_SetHeroFormation Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_SetHeroFormation), isFromPool) as C2M_SetHeroFormation;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 0上阵 1下阵
        /// </summary>
        [MemoryPackOrder(1)]
        public int OpType { get; set; }

        [MemoryPackOrder(2)]
        public long HeroId { get; set; }

        [MemoryPackOrder(4)]
        public int SlotIndex { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.OpType = default;
            this.HeroId = default;
            this.SlotIndex = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_SetHeroFormation)]
    public partial class M2C_SetHeroFormation : MessageObject, ILocationResponse
    {
        public static M2C_SetHeroFormation Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_SetHeroFormation), isFromPool) as M2C_SetHeroFormation;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<long> Formation { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.Formation.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_SetHeroEquipment)]
    [ResponseType(nameof(M2C_SetHeroEquipment))]
    public partial class C2M_SetHeroEquipment : MessageObject, ILocationRequest
    {
        public static C2M_SetHeroEquipment Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_SetHeroEquipment), isFromPool) as C2M_SetHeroEquipment;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 0穿上 1卸下
        /// </summary>
        [MemoryPackOrder(1)]
        public int OpType { get; set; }

        [MemoryPackOrder(4)]
        public long HeroId { get; set; }

        [MemoryPackOrder(5)]
        public long ItemId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.OpType = default;
            this.HeroId = default;
            this.ItemId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_SetHeroEquipment)]
    public partial class M2C_SetHeroEquipment : MessageObject, ILocationResponse
    {
        public static M2C_SetHeroEquipment Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_SetHeroEquipment), isFromPool) as M2C_SetHeroEquipment;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_RoleDataUpdate)]
    public partial class M2C_RoleDataUpdate : MessageObject, IMessage
    {
        public static M2C_RoleDataUpdate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_RoleDataUpdate), isFromPool) as M2C_RoleDataUpdate;
        }

        [MemoryPackOrder(0)]
        public int UpdateType { get; set; }

        [MemoryPackOrder(1)]
        public string UpdateTypeValue { get; set; }

        [MemoryPackOrder(2)]
        public long UpdateValueLong { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UpdateType = default;
            this.UpdateTypeValue = default;
            this.UpdateValueLong = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_GetUserInfo)]
    [ResponseType(nameof(M2C_GetUserInfo))]
    public partial class C2M_GetUserInfo : MessageObject, ILocationRequest
    {
        public static C2M_GetUserInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_GetUserInfo), isFromPool) as C2M_GetUserInfo;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_GetUserInfo)]
    public partial class M2C_GetUserInfo : MessageObject, ILocationResponse
    {
        public static M2C_GetUserInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_GetUserInfo), isFromPool) as M2C_GetUserInfo;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public string PlayerName { get; set; }

        [MemoryPackOrder(4)]
        public long Gold { get; set; }

        [MemoryPackOrder(5)]
        public long Diamond { get; set; }

        [MemoryPackOrder(6)]
        public long Exp { get; set; }

        [MemoryPackOrder(7)]
        public int Lv { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.PlayerName = default;
            this.Gold = default;
            this.Diamond = default;
            this.Exp = default;
            this.Lv = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_SetTimeScale)]
    [ResponseType(nameof(M2C_SetTimeScale))]
    public partial class C2M_SetTimeScale : MessageObject, ILocationRequest
    {
        public static C2M_SetTimeScale Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_SetTimeScale), isFromPool) as C2M_SetTimeScale;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public float TimeScale { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.TimeScale = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_SetTimeScale)]
    public partial class M2C_SetTimeScale : MessageObject, ILocationResponse
    {
        public static M2C_SetTimeScale Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_SetTimeScale), isFromPool) as M2C_SetTimeScale;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UpdateTimeScale)]
    public partial class M2C_UpdateTimeScale : MessageObject, IMessage
    {
        public static M2C_UpdateTimeScale Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UpdateTimeScale), isFromPool) as M2C_UpdateTimeScale;
        }

        [MemoryPackOrder(0)]
        public int Error { get; set; }

        [MemoryPackOrder(1)]
        public float TimeScale { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Error = default;
            this.TimeScale = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_EnterBossRoom)]
    [ResponseType(nameof(M2C_EnterBossRoom))]
    public partial class C2M_EnterBossRoom : MessageObject, ILocationRequest
    {
        public static C2M_EnterBossRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_EnterBossRoom), isFromPool) as C2M_EnterBossRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_EnterBossRoom)]
    public partial class M2C_EnterBossRoom : MessageObject, ILocationResponse
    {
        public static M2C_EnterBossRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_EnterBossRoom), isFromPool) as M2C_EnterBossRoom;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_SetAutoFight)]
    [ResponseType(nameof(M2C_SetAutoFight))]
    public partial class C2M_SetAutoFight : MessageObject, ILocationRequest
    {
        public static C2M_SetAutoFight Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_SetAutoFight), isFromPool) as C2M_SetAutoFight;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Value { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Value = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_SetAutoFight)]
    public partial class M2C_SetAutoFight : MessageObject, ILocationResponse
    {
        public static M2C_SetAutoFight Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_SetAutoFight), isFromPool) as M2C_SetAutoFight;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_TryUseSkill)]
    [ResponseType(nameof(M2C_TryUseSkill))]
    public partial class C2M_TryUseSkill : MessageObject, ILocationRequest
    {
        public static C2M_TryUseSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_TryUseSkill), isFromPool) as C2M_TryUseSkill;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int SkillConfigId { get; set; }

        [MemoryPackOrder(2)]
        public long TargetId { get; set; }

        [MemoryPackOrder(3)]
        public float Angle { get; set; }

        [MemoryPackOrder(4)]
        public Unity.Mathematics.float3 Position { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.SkillConfigId = default;
            this.TargetId = default;
            this.Angle = default;
            this.Position = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_TryUseSkill)]
    public partial class M2C_TryUseSkill : MessageObject, ILocationResponse
    {
        public static M2C_TryUseSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_TryUseSkill), isFromPool) as M2C_TryUseSkill;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_HeroUseSkill)]
    [ResponseType(nameof(M2C_HeroUseSkill))]
    public partial class C2M_HeroUseSkill : MessageObject, ILocationRequest
    {
        public static C2M_HeroUseSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_HeroUseSkill), isFromPool) as C2M_HeroUseSkill;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long HeroUnitId { get; set; }

        [MemoryPackOrder(2)]
        public int SkillConfigId { get; set; }

        [MemoryPackOrder(3)]
        public long TargetId { get; set; }

        [MemoryPackOrder(4)]
        public float Angle { get; set; }

        [MemoryPackOrder(5)]
        public Unity.Mathematics.float3 Position { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.HeroUnitId = default;
            this.SkillConfigId = default;
            this.TargetId = default;
            this.Angle = default;
            this.Position = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_HeroUseSkill)]
    public partial class M2C_HeroUseSkill : MessageObject, ILocationResponse
    {
        public static M2C_HeroUseSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_HeroUseSkill), isFromPool) as M2C_HeroUseSkill;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_OnUseSkill)]
    public partial class M2C_OnUseSkill : MessageObject, IMessage
    {
        public static M2C_OnUseSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_OnUseSkill), isFromPool) as M2C_OnUseSkill;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long SkillId { get; set; }

        [MemoryPackOrder(2)]
        public int SkillConfigId { get; set; }

        [MemoryPackOrder(3)]
        public long TargetId { get; set; }

        [MemoryPackOrder(4)]
        public float Angle { get; set; }

        [MemoryPackOrder(5)]
        public Unity.Mathematics.float3 Position { get; set; }

        [MemoryPackOrder(6)]
        public float CD { get; set; }

        [MemoryPackOrder(7)]
        public float PublicCD { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.SkillId = default;
            this.SkillConfigId = default;
            this.TargetId = default;
            this.Angle = default;
            this.Position = default;
            this.CD = default;
            this.PublicCD = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UnitSkillRemove)]
    public partial class M2C_UnitSkillRemove : MessageObject, IMessage
    {
        public static M2C_UnitSkillRemove Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UnitSkillRemove), isFromPool) as M2C_UnitSkillRemove;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long SkillId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.SkillId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UnitFinishSkill)]
    public partial class M2C_UnitFinishSkill : MessageObject, IMessage
    {
        public static M2C_UnitFinishSkill Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UnitFinishSkill), isFromPool) as M2C_UnitFinishSkill;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UnitBuffUpdate)]
    public partial class M2C_UnitBuffUpdate : MessageObject, IMessage
    {
        public static M2C_UnitBuffUpdate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UnitBuffUpdate), isFromPool) as M2C_UnitBuffUpdate;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long BuffId { get; set; }

        [MemoryPackOrder(2)]
        public int BuffConfigId { get; set; }

        /// <summary>
        /// 1新增  2移除 3重置
        /// </summary>
        [MemoryPackOrder(3)]
        public int BuffOperateType { get; set; }

        [MemoryPackOrder(4)]
        public List<float> TargetPostion { get; set; } = new();

        [MemoryPackOrder(5)]
        public float BuffEndTime { get; set; }

        [MemoryPackOrder(6)]
        public string Spellcaster { get; set; }

        [MemoryPackOrder(7)]
        public int UnitType { get; set; }

        [MemoryPackOrder(8)]
        public int UnitConfigId { get; set; }

        [MemoryPackOrder(9)]
        public int SkillConfigId { get; set; }

        [MemoryPackOrder(10)]
        public long UnitIdFrom { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.BuffId = default;
            this.BuffConfigId = default;
            this.BuffOperateType = default;
            this.TargetPostion.Clear();
            this.BuffEndTime = default;
            this.Spellcaster = default;
            this.UnitType = default;
            this.UnitConfigId = default;
            this.SkillConfigId = default;
            this.UnitIdFrom = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UnitBuffRemove)]
    public partial class M2C_UnitBuffRemove : MessageObject, IMessage
    {
        public static M2C_UnitBuffRemove Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UnitBuffRemove), isFromPool) as M2C_UnitBuffRemove;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long BuffId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.BuffId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_UnitStateUpdate)]
    public partial class M2C_UnitStateUpdate : MessageObject, IMessage
    {
        public static M2C_UnitStateUpdate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_UnitStateUpdate), isFromPool) as M2C_UnitStateUpdate;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long StateType { get; set; }

        [MemoryPackOrder(2)]
        public int StateOperateType { get; set; }

        [MemoryPackOrder(3)]
        public int StateTime { get; set; }

        [MemoryPackOrder(4)]
        public string StateValue { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.StateType = default;
            this.StateOperateType = default;
            this.StateTime = default;
            this.StateValue = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.TaskProInfo)]
    public partial class TaskProInfo : MessageObject
    {
        public static TaskProInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(TaskProInfo), isFromPool) as TaskProInfo;
        }

        [MemoryPackOrder(0)]
        public long Id { get; set; }

        [MemoryPackOrder(1)]
        public int ConfigId { get; set; }

        [MemoryPackOrder(2)]
        public int TaskState { get; set; }

        [MemoryPackOrder(3)]
        public int TaskTargetNum_1 { get; set; }

        [MemoryPackOrder(4)]
        public int TaskTargetNum_2 { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Id = default;
            this.ConfigId = default;
            this.TaskState = default;
            this.TaskTargetNum_1 = default;
            this.TaskTargetNum_2 = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_GetAllTask)]
    [ResponseType(nameof(M2C_GetAllTask))]
    public partial class C2M_GetAllTask : MessageObject, ILocationRequest
    {
        public static C2M_GetAllTask Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_GetAllTask), isFromPool) as C2M_GetAllTask;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_GetAllTask)]
    public partial class M2C_GetAllTask : MessageObject, ILocationResponse
    {
        public static M2C_GetAllTask Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_GetAllTask), isFromPool) as M2C_GetAllTask;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<TaskProInfo> TaskProInfoList { get; set; } = new();

        [MemoryPackOrder(4)]
        public List<int> CompleteTaskList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.TaskProInfoList.Clear();
            this.CompleteTaskList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_TaskUpdate)]
    public partial class M2C_TaskUpdate : MessageObject, IMessage
    {
        public static M2C_TaskUpdate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_TaskUpdate), isFromPool) as M2C_TaskUpdate;
        }

        /// <summary>
        /// 0全量  2增量
        /// </summary>
        [MemoryPackOrder(0)]
        public int UpdateMode { get; set; }

        [MemoryPackOrder(1)]
        public List<TaskProInfo> TaskProInfoList { get; set; } = new();

        [MemoryPackOrder(2)]
        public List<int> CompleteTaskList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UpdateMode = default;
            this.TaskProInfoList.Clear();
            this.CompleteTaskList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_TaskCommit)]
    [ResponseType(nameof(M2C_TaskCommit))]
    public partial class C2M_TaskCommit : MessageObject, ILocationRequest
    {
        public static C2M_TaskCommit Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_TaskCommit), isFromPool) as C2M_TaskCommit;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int TaskConfigId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.TaskConfigId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_TaskCommit)]
    public partial class M2C_TaskCommit : MessageObject, ILocationResponse
    {
        public static M2C_TaskCommit Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_TaskCommit), isFromPool) as M2C_TaskCommit;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<TaskProInfo> TaskProInfoList { get; set; } = new();

        [MemoryPackOrder(4)]
        public List<int> CompleteTaskList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.TaskProInfoList.Clear();
            this.CompleteTaskList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_PickUpDropItem)]
    [ResponseType(nameof(M2C_PickUpDropItem))]
    public partial class C2M_PickUpDropItem : MessageObject, ILocationRequest
    {
        public static C2M_PickUpDropItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_PickUpDropItem), isFromPool) as C2M_PickUpDropItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public List<long> UnitIdList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitIdList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_PickUpDropItem)]
    public partial class M2C_PickUpDropItem : MessageObject, ILocationResponse
    {
        public static M2C_PickUpDropItem Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_PickUpDropItem), isFromPool) as M2C_PickUpDropItem;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_WatchPlayer)]
    [ResponseType(nameof(M2C_WatchPlayer))]
    public partial class C2M_WatchPlayer : MessageObject, ILocationRequest
    {
        public static C2M_WatchPlayer Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_WatchPlayer), isFromPool) as C2M_WatchPlayer;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.WatchPlayerInfo)]
    public partial class WatchPlayerInfo : MessageObject
    {
        public static WatchPlayerInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(WatchPlayerInfo), isFromPool) as WatchPlayerInfo;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public string PlayerName { get; set; }

        [MemoryPackOrder(2)]
        public string AllianceName { get; set; }

        [MemoryPackOrder(3)]
        public long CombatPower { get; set; }

        [MemoryPackOrder(4)]
        public List<long> HeroFormation { get; set; } = new();

        [MemoryPackOrder(5)]
        public List<HeroInfo> HeroInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.PlayerName = default;
            this.AllianceName = default;
            this.CombatPower = default;
            this.HeroFormation.Clear();
            this.HeroInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_WatchPlayer)]
    public partial class M2C_WatchPlayer : MessageObject, ILocationResponse
    {
        public static M2C_WatchPlayer Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_WatchPlayer), isFromPool) as M2C_WatchPlayer;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public WatchPlayerInfo WatchPlayerInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.WatchPlayerInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.MailInfo)]
    public partial class MailInfo : MessageObject
    {
        public static MailInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(MailInfo), isFromPool) as MailInfo;
        }

        [MemoryPackOrder(0)]
        public long Id { get; set; }

        [MemoryPackOrder(1)]
        public string From { get; set; }

        [MemoryPackOrder(2)]
        public string Title { get; set; }

        [MemoryPackOrder(3)]
        public string Content { get; set; }

        [MemoryPackOrder(4)]
        public long Time { get; set; }

        [MemoryPackOrder(5)]
        public long EndTime { get; set; }

        [MemoryPackOrder(6)]
        public int MailReadState { get; set; }

        [MemoryPackOrder(7)]
        public int MailRewardState { get; set; }

        [MemoryPackOrder(8)]
        public int MailDeleteState { get; set; }

        [MemoryPackOrder(9)]
        public MailRewardComponentInfo MailRewardComponentInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Id = default;
            this.From = default;
            this.Title = default;
            this.Content = default;
            this.Time = default;
            this.EndTime = default;
            this.MailReadState = default;
            this.MailRewardState = default;
            this.MailDeleteState = default;
            this.MailRewardComponentInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.MailRewardComponentInfo)]
    public partial class MailRewardComponentInfo : MessageObject
    {
        public static MailRewardComponentInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(MailRewardComponentInfo), isFromPool) as MailRewardComponentInfo;
        }

        [MemoryPackOrder(0)]
        public List<ItemInfo> ItemInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ItemInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Mail_GetAllMailList)]
    [ResponseType(nameof(Mail2C_GetAllMailList))]
    public partial class C2Mail_GetAllMailList : MessageObject, IMailRequest
    {
        public static C2Mail_GetAllMailList Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Mail_GetAllMailList), isFromPool) as C2Mail_GetAllMailList;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Mail2C_GetAllMailList)]
    public partial class Mail2C_GetAllMailList : MessageObject, IMailResponse
    {
        public static Mail2C_GetAllMailList Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Mail2C_GetAllMailList), isFromPool) as Mail2C_GetAllMailList;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        [MemoryPackOrder(0)]
        public List<MailInfo> MailInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.MailInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Mail_OpeMail)]
    [ResponseType(nameof(Mail2C_OpeMail))]
    public partial class C2Mail_OpeMail : MessageObject, IMailRequest
    {
        public static C2Mail_OpeMail Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Mail_OpeMail), isFromPool) as C2Mail_OpeMail;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(0)]
        public int MailOpType { get; set; }

        [MemoryPackOrder(1)]
        public List<long> MailId { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.MailOpType = default;
            this.MailId.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Mail2C_OpeMail)]
    public partial class Mail2C_OpeMail : MessageObject, IMailResponse
    {
        public static Mail2C_OpeMail Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Mail2C_OpeMail), isFromPool) as Mail2C_OpeMail;
        }

        [MemoryPackOrder(89)]
        public int RpcId { get; set; }

        [MemoryPackOrder(90)]
        public int Error { get; set; }

        [MemoryPackOrder(91)]
        public string Message { get; set; }

        [MemoryPackOrder(0)]
        public List<MailInfo> MailInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.MailInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Mail2C_ReceiveMail)]
    public partial class Mail2C_ReceiveMail : MessageObject, IMessage
    {
        public static Mail2C_ReceiveMail Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Mail2C_ReceiveMail), isFromPool) as Mail2C_ReceiveMail;
        }

        [MemoryPackOrder(0)]
        public MailInfo MailInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.MailInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_NoticeUnitTransformList)]
    public partial class M2C_NoticeUnitTransformList : MessageObject, IMessage
    {
        public static M2C_NoticeUnitTransformList Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_NoticeUnitTransformList), isFromPool) as M2C_NoticeUnitTransformList;
        }

        [MemoryPackOrder(0)]
        public List<long> UnitIdList { get; set; } = new();

        [MemoryPackOrder(1)]
        public List<Unity.Mathematics.float3> PositionList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitIdList.Clear();
            this.PositionList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_NoticeUnitTransform)]
    public partial class C2M_NoticeUnitTransform : MessageObject, ILocationMessage
    {
        public static C2M_NoticeUnitTransform Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_NoticeUnitTransform), isFromPool) as C2M_NoticeUnitTransform;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public Unity.Mathematics.float3 Position { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Position = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.ChatRoomInfo)]
    public partial class ChatRoomInfo : MessageObject
    {
        public static ChatRoomInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(ChatRoomInfo), isFromPool) as ChatRoomInfo;
        }

        [MemoryPackOrder(0)]
        public string ChatRoomKey { get; set; }

        [MemoryPackOrder(1)]
        public int ChatRoomType { get; set; }

        [MemoryPackOrder(2)]
        public List<long> UnitList { get; set; } = new();

        [MemoryPackOrder(3)]
        public List<ChatInfo> ChatInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ChatRoomKey = default;
            this.ChatRoomType = default;
            this.UnitList.Clear();
            this.ChatInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.ChatInfo)]
    public partial class ChatInfo : MessageObject
    {
        public static ChatInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(ChatInfo), isFromPool) as ChatInfo;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public long SendTime { get; set; }

        [MemoryPackOrder(2)]
        public string Name { get; set; }

        [MemoryPackOrder(3)]
        public string Content { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.SendTime = default;
            this.Name = default;
            this.Content = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Chat_GetAllChat)]
    [ResponseType(nameof(Chat2C_GetAllChat))]
    public partial class C2Chat_GetAllChat : MessageObject, IChatRequest
    {
        public static C2Chat_GetAllChat Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Chat_GetAllChat), isFromPool) as C2Chat_GetAllChat;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Chat2C_GetAllChat)]
    public partial class Chat2C_GetAllChat : MessageObject, IChatResponse
    {
        public static Chat2C_GetAllChat Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Chat2C_GetAllChat), isFromPool) as Chat2C_GetAllChat;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ChatRoomInfo> ChatRoomInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ChatRoomInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Chat2C_UpdateChatRoom)]
    public partial class Chat2C_UpdateChatRoom : MessageObject, IMessage
    {
        public static Chat2C_UpdateChatRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Chat2C_UpdateChatRoom), isFromPool) as Chat2C_UpdateChatRoom;
        }

        [MemoryPackOrder(0)]
        public ChatRoomInfo ChatRoomInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ChatRoomInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Chat_SendChat)]
    [ResponseType(nameof(Chat2C_SendChat))]
    public partial class C2Chat_SendChat : MessageObject, IChatRequest
    {
        public static C2Chat_SendChat Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Chat_SendChat), isFromPool) as C2Chat_SendChat;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public string ChatRoomKey { get; set; }

        [MemoryPackOrder(2)]
        public string Content { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ChatRoomKey = default;
            this.Content = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Chat2C_SendChat)]
    public partial class Chat2C_SendChat : MessageObject, IChatResponse
    {
        public static Chat2C_SendChat Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Chat2C_SendChat), isFromPool) as Chat2C_SendChat;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Chat_Report)]
    [ResponseType(nameof(Chat2C_Report))]
    public partial class C2Chat_Report : MessageObject, IChatRequest
    {
        public static C2Chat_Report Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Chat_Report), isFromPool) as C2Chat_Report;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Chat2C_Report)]
    public partial class Chat2C_Report : MessageObject, IChatResponse
    {
        public static Chat2C_Report Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Chat2C_Report), isFromPool) as Chat2C_Report;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Chat2C_NoticeChat)]
    public partial class Chat2C_NoticeChat : MessageObject, IMessage
    {
        public static Chat2C_NoticeChat Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Chat2C_NoticeChat), isFromPool) as Chat2C_NoticeChat;
        }

        [MemoryPackOrder(0)]
        public string ChatRoomKey { get; set; }

        [MemoryPackOrder(1)]
        public ChatInfo ChatInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.ChatRoomKey = default;
            this.ChatInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.FriendDataInfo)]
    public partial class FriendDataInfo : MessageObject
    {
        public static FriendDataInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(FriendDataInfo), isFromPool) as FriendDataInfo;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public int OnLine { get; set; }

        [MemoryPackOrder(2)]
        public long LastLoginTime { get; set; }

        [MemoryPackOrder(3)]
        public string PlayerName { get; set; }

        [MemoryPackOrder(4)]
        public int Lv { get; set; }

        [MemoryPackOrder(5)]
        public long CombatPower { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.OnLine = default;
            this.LastLoginTime = default;
            this.PlayerName = default;
            this.Lv = default;
            this.CombatPower = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Friend_GetAllFriend)]
    [ResponseType(nameof(Friend2C_GetAllFriend))]
    public partial class C2Friend_GetAllFriend : MessageObject, IFriendRequest
    {
        public static C2Friend_GetAllFriend Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Friend_GetAllFriend), isFromPool) as C2Friend_GetAllFriend;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_GetAllFriend)]
    public partial class Friend2C_GetAllFriend : MessageObject, IFriendResponse
    {
        public static Friend2C_GetAllFriend Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_GetAllFriend), isFromPool) as Friend2C_GetAllFriend;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<FriendDataInfo> FriendList { get; set; } = new();

        [MemoryPackOrder(4)]
        public List<FriendDataInfo> RequestList { get; set; } = new();

        [MemoryPackOrder(5)]
        public List<FriendDataInfo> BlackList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.FriendList.Clear();
            this.RequestList.Clear();
            this.BlackList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Friend_FriendRequest)]
    [ResponseType(nameof(Friend2C_FriendRequest))]
    public partial class C2Friend_FriendRequest : MessageObject, IFriendRequest
    {
        public static C2Friend_FriendRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Friend_FriendRequest), isFromPool) as C2Friend_FriendRequest;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_FriendRequest)]
    public partial class Friend2C_FriendRequest : MessageObject, IFriendResponse
    {
        public static Friend2C_FriendRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_FriendRequest), isFromPool) as Friend2C_FriendRequest;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_ReceiveFriendRequest)]
    public partial class Friend2C_ReceiveFriendRequest : MessageObject, IMessage
    {
        public static Friend2C_ReceiveFriendRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_ReceiveFriendRequest), isFromPool) as Friend2C_ReceiveFriendRequest;
        }

        [MemoryPackOrder(0)]
        public FriendDataInfo FriendDataInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.FriendDataInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Friend_FriendRequestAccept)]
    [ResponseType(nameof(Friend2C_FriendRequestAccept))]
    public partial class C2Friend_FriendRequestAccept : MessageObject, IFriendRequest
    {
        public static C2Friend_FriendRequestAccept Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Friend_FriendRequestAccept), isFromPool) as C2Friend_FriendRequestAccept;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long UnitId { get; set; }

        [MemoryPackOrder(2)]
        public int IsAgree { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;
            this.IsAgree = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_FriendRequestAccept)]
    public partial class Friend2C_FriendRequestAccept : MessageObject, IFriendResponse
    {
        public static Friend2C_FriendRequestAccept Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_FriendRequestAccept), isFromPool) as Friend2C_FriendRequestAccept;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_FriendRequestSucceed)]
    public partial class Friend2C_FriendRequestSucceed : MessageObject, IMessage
    {
        public static Friend2C_FriendRequestSucceed Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_FriendRequestSucceed), isFromPool) as Friend2C_FriendRequestSucceed;
        }

        [MemoryPackOrder(0)]
        public FriendDataInfo FriendDataInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.FriendDataInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Friend_DeleteFriend)]
    [ResponseType(nameof(Friend2C_DeleteFriend))]
    public partial class C2Friend_DeleteFriend : MessageObject, IFriendRequest
    {
        public static C2Friend_DeleteFriend Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Friend_DeleteFriend), isFromPool) as C2Friend_DeleteFriend;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_DeleteFriend)]
    public partial class Friend2C_DeleteFriend : MessageObject, IFriendResponse
    {
        public static Friend2C_DeleteFriend Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_DeleteFriend), isFromPool) as Friend2C_DeleteFriend;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Friend_BlackFriend)]
    [ResponseType(nameof(Friend2C_BlackFriend))]
    public partial class C2Friend_BlackFriend : MessageObject, IFriendRequest
    {
        public static C2Friend_BlackFriend Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Friend_BlackFriend), isFromPool) as C2Friend_BlackFriend;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public long UnitId { get; set; }

        /// <summary>
        /// 0拉黑 1取消拉黑
        /// </summary>
        [MemoryPackOrder(2)]
        public int Ope { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.UnitId = default;
            this.Ope = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_BlackFriend)]
    public partial class Friend2C_BlackFriend : MessageObject, IFriendResponse
    {
        public static Friend2C_BlackFriend Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_BlackFriend), isFromPool) as Friend2C_BlackFriend;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public FriendDataInfo FriendDataInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.FriendDataInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_DeleteYou)]
    public partial class Friend2C_DeleteYou : MessageObject, IMessage
    {
        public static Friend2C_DeleteYou Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_DeleteYou), isFromPool) as Friend2C_DeleteYou;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Friend2C_FriendOnLineChange)]
    public partial class Friend2C_FriendOnLineChange : MessageObject, IMessage
    {
        public static Friend2C_FriendOnLineChange Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Friend2C_FriendOnLineChange), isFromPool) as Friend2C_FriendOnLineChange;
        }

        [MemoryPackOrder(0)]
        public long UnitId { get; set; }

        [MemoryPackOrder(1)]
        public int OnLine { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.UnitId = default;
            this.OnLine = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.RankDataInfo)]
    public partial class RankDataInfo : MessageObject
    {
        public static RankDataInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(RankDataInfo), isFromPool) as RankDataInfo;
        }

        [MemoryPackOrder(0)]
        public int RankType { get; set; }

        [MemoryPackOrder(1)]
        public int Rank { get; set; }

        [MemoryPackOrder(2)]
        public long UnitId { get; set; }

        [MemoryPackOrder(3)]
        public string PlayerName { get; set; }

        [MemoryPackOrder(4)]
        public long CombatPower { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RankType = default;
            this.Rank = default;
            this.UnitId = default;
            this.PlayerName = default;
            this.CombatPower = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2Rank_GetAllRank)]
    [ResponseType(nameof(Rank2C_GetAllRank))]
    public partial class C2Rank_GetAllRank : MessageObject, IRankRequest
    {
        public static C2Rank_GetAllRank Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Rank_GetAllRank), isFromPool) as C2Rank_GetAllRank;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Rank2C_GetAllRank)]
    public partial class Rank2C_GetAllRank : MessageObject, IRankResponse
    {
        public static Rank2C_GetAllRank Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Rank2C_GetAllRank), isFromPool) as Rank2C_GetAllRank;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<RankDataInfo> RankDataList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.RankDataList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.Rank2C_NoticeRankUpdate)]
    public partial class Rank2C_NoticeRankUpdate : MessageObject, IMessage
    {
        public static Rank2C_NoticeRankUpdate Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Rank2C_NoticeRankUpdate), isFromPool) as Rank2C_NoticeRankUpdate;
        }

        [MemoryPackOrder(0)]
        public List<RankDataInfo> RankDataInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RankDataInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.C2M_LotteryDrawRequest)]
    [ResponseType(nameof(M2C_LotteryDrawRequest))]
    public partial class C2M_LotteryDrawRequest : MessageObject, ILocationRequest
    {
        public static C2M_LotteryDrawRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2M_LotteryDrawRequest), isFromPool) as C2M_LotteryDrawRequest;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 0单抽 1十连抽
        /// </summary>
        [MemoryPackOrder(1)]
        public int OpType { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.OpType = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.M2C_LotteryDrawRequest)]
    public partial class M2C_LotteryDrawRequest : MessageObject, ILocationResponse
    {
        public static M2C_LotteryDrawRequest Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(M2C_LotteryDrawRequest), isFromPool) as M2C_LotteryDrawRequest;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        [MemoryPackOrder(3)]
        public List<ItemInfo> ItemInfoList { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;
            this.ItemInfoList.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(OuterMessage.ArchiveHeroInfo)]
    public partial class ArchiveHeroInfo : MessageObject
    {
        public static ArchiveHeroInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(ArchiveHeroInfo), isFromPool) as ArchiveHeroInfo;
        }

        [MemoryPackOrder(0)]
        public int HeroConfigId { get; set; }

        [MemoryPackOrder(1)]
        public int Lv { get; set; }

        [MemoryPackOrder(2)]
        public int Star { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.HeroConfigId = default;
            this.Lv = default;
            this.Star = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class OuterMessage
    {
        public const ushort HttpGetRouterResponse = 10002;
        public const ushort RouterSync = 10003;
        public const ushort C2M_TestRequest = 10004;
        public const ushort M2C_TestResponse = 10005;
        public const ushort C2G_EnterGame = 10006;
        public const ushort G2C_EnterGame = 10007;
        public const ushort MoveInfo = 10008;
        public const ushort UnitInfo = 10009;
        public const ushort M2C_CreateUnits = 10010;
        public const ushort M2C_CreateMyUnit = 10011;
        public const ushort M2C_StartSceneChange = 10012;
        public const ushort M2C_RemoveUnits = 10013;
        public const ushort C2M_PathfindingRequest = 10014;
        public const ushort C2M_PathfindingResult = 10015;
        public const ushort C2M_Stop = 10016;
        public const ushort M2C_Stop = 10017;
        public const ushort C2M_StopResult = 10018;
        public const ushort M2C_StopResult = 10019;
        public const ushort M2C_PathfindingResult = 10020;
        public const ushort C2G_Ping = 10021;
        public const ushort G2C_Ping = 10022;
        public const ushort C2R_Ping = 10023;
        public const ushort R2C_Ping = 10024;
        public const ushort G2C_Test = 10025;
        public const ushort C2M_Reload = 10026;
        public const ushort M2C_Reload = 10027;
        public const ushort ServerItem = 10028;
        public const ushort C2R_DeleteAccountRequest = 10029;
        public const ushort R2C_DeleteAccountResponse = 10030;
        public const ushort C2R_ServerList = 10031;
        public const ushort R2C_ServerList = 10032;
        public const ushort C2R_LoginAccount = 10033;
        public const ushort R2C_LoginAccount = 10034;
        public const ushort C2R_RealNameRequest = 10035;
        public const ushort R2C_RealNameResponse = 10036;
        public const ushort C2M_RealNameRequest = 10037;
        public const ushort M2C_RealNameResponse = 10038;
        public const ushort RechargeInfo = 10039;
        public const ushort PlayerInfo = 10040;
        public const ushort CreateRoleInfo = 10041;
        public const ushort C2G_LoginGameGate = 10042;
        public const ushort G2C_LoginGameGate = 10043;
        public const ushort C2R_CreateRoleData = 10044;
        public const ushort R2C_CreateRoleData = 10045;
        public const ushort C2R_DeleteRoleData = 10046;
        public const ushort R2C_DeleteRoleData = 10047;
        public const ushort C2Q_EnterQueue = 10048;
        public const ushort Q2C_EnterQueue = 10049;
        public const ushort C2R_GetRealmKey = 10050;
        public const ushort R2C_GetRealmKey = 10051;
        public const ushort G2C_TestHotfixMessage = 10052;
        public const ushort C2M_TestRobotCase = 10053;
        public const ushort M2C_TestRobotCase = 10054;
        public const ushort C2M_TestRobotCase2 = 10055;
        public const ushort M2C_TestRobotCase2 = 10056;
        public const ushort C2M_TransferMap = 10057;
        public const ushort M2C_TransferMap = 10058;
        public const ushort C2G_Benchmark = 10059;
        public const ushort G2C_Benchmark = 10060;
        public const ushort ServerInfo = 10061;
        public const ushort A2C_Disconnect = 10062;
        public const ushort G2C_SecondLogin = 10063;
        public const ushort M2C_UnitNumericListUpdate = 10064;
        public const ushort M2C_UnitNumericUpdate = 10065;
        public const ushort M2C_RoleDataBroadcast = 10066;
        public const ushort C2M_GMCommand = 10067;
        public const ushort ItemInfo = 10068;
        public const ushort C2M_GetAllItem = 10069;
        public const ushort M2C_GetAllItem = 10070;
        public const ushort M2C_ItemUpdateOp = 10071;
        public const ushort C2M_SellItem = 10072;
        public const ushort M2C_SellItem = 10073;
        public const ushort C2M_UseItem = 10074;
        public const ushort M2C_UseItem = 10075;
        public const ushort C2M_MoveItem = 10076;
        public const ushort M2C_MoveItem = 10077;
        public const ushort C2M_GetStoreInfo = 10078;
        public const ushort M2C_GetStoreInfo = 10079;
        public const ushort C2M_StoreBuy = 10080;
        public const ushort M2C_StoreBuy = 10081;
        public const ushort C2M_RefreshStore = 10082;
        public const ushort M2C_RefreshStore = 10083;
        public const ushort HeroInfo = 10084;
        public const ushort C2M_GetAllHero = 10085;
        public const ushort M2C_GetAllHero = 10086;
        public const ushort M2C_HeroUpdateOp = 10087;
        public const ushort C2M_SetHeroFormation = 10088;
        public const ushort M2C_SetHeroFormation = 10089;
        public const ushort C2M_SetHeroEquipment = 10090;
        public const ushort M2C_SetHeroEquipment = 10091;
        public const ushort M2C_RoleDataUpdate = 10092;
        public const ushort C2M_GetUserInfo = 10093;
        public const ushort M2C_GetUserInfo = 10094;
        public const ushort C2M_SetTimeScale = 10095;
        public const ushort M2C_SetTimeScale = 10096;
        public const ushort M2C_UpdateTimeScale = 10097;
        public const ushort C2M_EnterBossRoom = 10098;
        public const ushort M2C_EnterBossRoom = 10099;
        public const ushort C2M_SetAutoFight = 10100;
        public const ushort M2C_SetAutoFight = 10101;
        public const ushort C2M_TryUseSkill = 10102;
        public const ushort M2C_TryUseSkill = 10103;
        public const ushort C2M_HeroUseSkill = 10104;
        public const ushort M2C_HeroUseSkill = 10105;
        public const ushort M2C_OnUseSkill = 10106;
        public const ushort M2C_UnitSkillRemove = 10107;
        public const ushort M2C_UnitFinishSkill = 10108;
        public const ushort M2C_UnitBuffUpdate = 10109;
        public const ushort M2C_UnitBuffRemove = 10110;
        public const ushort M2C_UnitStateUpdate = 10111;
        public const ushort TaskProInfo = 10112;
        public const ushort C2M_GetAllTask = 10113;
        public const ushort M2C_GetAllTask = 10114;
        public const ushort M2C_TaskUpdate = 10115;
        public const ushort C2M_TaskCommit = 10116;
        public const ushort M2C_TaskCommit = 10117;
        public const ushort C2M_PickUpDropItem = 10118;
        public const ushort M2C_PickUpDropItem = 10119;
        public const ushort C2M_WatchPlayer = 10120;
        public const ushort WatchPlayerInfo = 10121;
        public const ushort M2C_WatchPlayer = 10122;
        public const ushort MailInfo = 10123;
        public const ushort MailRewardComponentInfo = 10124;
        public const ushort C2Mail_GetAllMailList = 10125;
        public const ushort Mail2C_GetAllMailList = 10126;
        public const ushort C2Mail_OpeMail = 10127;
        public const ushort Mail2C_OpeMail = 10128;
        public const ushort Mail2C_ReceiveMail = 10129;
        public const ushort M2C_NoticeUnitTransformList = 10130;
        public const ushort C2M_NoticeUnitTransform = 10131;
        public const ushort ChatRoomInfo = 10132;
        public const ushort ChatInfo = 10133;
        public const ushort C2Chat_GetAllChat = 10134;
        public const ushort Chat2C_GetAllChat = 10135;
        public const ushort Chat2C_UpdateChatRoom = 10136;
        public const ushort C2Chat_SendChat = 10137;
        public const ushort Chat2C_SendChat = 10138;
        public const ushort C2Chat_Report = 10139;
        public const ushort Chat2C_Report = 10140;
        public const ushort Chat2C_NoticeChat = 10141;
        public const ushort FriendDataInfo = 10142;
        public const ushort C2Friend_GetAllFriend = 10143;
        public const ushort Friend2C_GetAllFriend = 10144;
        public const ushort C2Friend_FriendRequest = 10145;
        public const ushort Friend2C_FriendRequest = 10146;
        public const ushort Friend2C_ReceiveFriendRequest = 10147;
        public const ushort C2Friend_FriendRequestAccept = 10148;
        public const ushort Friend2C_FriendRequestAccept = 10149;
        public const ushort Friend2C_FriendRequestSucceed = 10150;
        public const ushort C2Friend_DeleteFriend = 10151;
        public const ushort Friend2C_DeleteFriend = 10152;
        public const ushort C2Friend_BlackFriend = 10153;
        public const ushort Friend2C_BlackFriend = 10154;
        public const ushort Friend2C_DeleteYou = 10155;
        public const ushort Friend2C_FriendOnLineChange = 10156;
        public const ushort RankDataInfo = 10157;
        public const ushort C2Rank_GetAllRank = 10158;
        public const ushort Rank2C_GetAllRank = 10159;
        public const ushort Rank2C_NoticeRankUpdate = 10160;
        public const ushort C2M_LotteryDrawRequest = 10161;
        public const ushort M2C_LotteryDrawRequest = 10162;
        public const ushort ArchiveHeroInfo = 10163;
    }
}