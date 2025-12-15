using System.Collections.Generic;
using DotRecast.Core.Numerics;
using DotRecast.Detour;
using DotRecast.Detour.Crowd;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class CrowdComponent : Entity, IAwake<string>, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public Dictionary<int, EntityRef<Unit>> agent2unit = new();

        public DtNavMesh navMesh;

        public DtCrowdAgentConfig _agCfg;
        public DtCrowdAgentDebugInfo _agentDebug;
        public DtCrowd crowd;
    }
}