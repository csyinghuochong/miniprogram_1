using System.Collections.Generic;
using DotRecast.Core.Numerics;
using DotRecast.Detour;
using DotRecast.Detour.Crowd;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class CrowdComponent : Entity, IAwake<byte[]>, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public Dictionary<int, EntityRef<Unit>> Agent2Unit = new();
        public HashSet<int> ToRemoveAgents = new();

        public DtNavMesh NavMesh;

        public DtCrowdAgentConfig AgCfg;
        public DtCrowdAgentDebugInfo AgentDebug;
        public DtCrowd Crowd;
    }
}