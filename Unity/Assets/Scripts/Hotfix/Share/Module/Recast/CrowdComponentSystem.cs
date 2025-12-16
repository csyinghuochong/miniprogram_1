using System;
using System.IO;
using DotRecast.Core;
using DotRecast.Core.Numerics;
using DotRecast.Detour;
using DotRecast.Detour.Crowd;
using DotRecast.Detour.Io;
using Unity.Mathematics;

namespace ET
{
    [EntitySystemOf(typeof(CrowdComponent))]
    [FriendOf(typeof(CrowdComponent))]
    public static partial class CrowdComponentSystem
    {
        [Invoke(TimerInvokeType.CrowdTimer)]
        public class CrowdTimer : ATimer<CrowdComponent>
        {
            protected override void Run(CrowdComponent self)
            {
                try
                {
                    self.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        const float agentRadius = 0.6f;
        const float agentHeight = 2f;
        private const float agentMaxAcceleration = 8f;
        const int SAMPLE_POLYFLAGS_DISABLED = 0x10; // Disabled polygon
        const int SAMPLE_POLYFLAGS_ALL = 0xffff; // All abilities.

        [EntitySystem]
        private static void Awake(this CrowdComponent self, string name)
        {
            byte[] buffer = NavmeshComponent.Instance.Get(name);
            DtMeshSetReader reader = new();
            using MemoryStream ms = new(buffer);
            using BinaryReader br = new(ms);
            self.navMesh = reader.Read32Bit(br, 6); // cpp recast导出来的要用Read32Bit读取，DotRecast导出来的还没试过

            if (self.navMesh == null)
            {
                throw new Exception($"nav load fail: {name}");
            }

            self._agCfg = new DtCrowdAgentConfig();
            self._agentDebug = new DtCrowdAgentDebugInfo();
            self._agentDebug.vod = new DtObstacleAvoidanceDebugData(2048);

            self.Setup(self.navMesh);

            self.LastUpdateTime = TimeInfo.Instance.ClientNow();
            self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.CrowdTimer, self);
        }

        [EntitySystem]
        private static void Destroy(this CrowdComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.agent2unit.Clear();
            self.toRemoveAgents.Clear();
            self.navMesh = null;
            self.crowd = null;
        }

        private static void Setup(this CrowdComponent self, DtNavMesh navMesh)
        {
            DtCrowdConfig config = new DtCrowdConfig(agentRadius);
            self.crowd = new DtCrowd(config, navMesh, __ => new DtQueryDefaultFilter(SAMPLE_POLYFLAGS_ALL,
                SAMPLE_POLYFLAGS_DISABLED,
                new float[] { 1f, 10f, 1f, 1f, 2f, 1.5f }));

            // Setup local avoidance option to different qualities.
            // Use mostly default settings, copy from dtCrowd.
            DtObstacleAvoidanceParams option = new DtObstacleAvoidanceParams(self.crowd.GetObstacleAvoidanceParams(0));

            // Low (11)
            option.velBias = 0.5f;
            option.adaptiveDivs = 5;
            option.adaptiveRings = 2;
            option.adaptiveDepth = 1;
            self.crowd.SetObstacleAvoidanceParams(0, option);

            // Medium (22)
            option.velBias = 0.5f;
            option.adaptiveDivs = 5;
            option.adaptiveRings = 2;
            option.adaptiveDepth = 2;
            self.crowd.SetObstacleAvoidanceParams(1, option);

            // Good (45)
            option.velBias = 0.5f;
            option.adaptiveDivs = 7;
            option.adaptiveRings = 2;
            option.adaptiveDepth = 3;
            self.crowd.SetObstacleAvoidanceParams(2, option);

            // High (66)
            option.velBias = 0.5f;
            option.adaptiveDivs = 7;
            option.adaptiveRings = 3;
            option.adaptiveDepth = 3;

            self.crowd.SetObstacleAvoidanceParams(3, option);
        }

        private static void Update(this CrowdComponent self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float dt = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            if (self.crowd == null)
                return;

            DtNavMesh nav = self.crowd.GetNavMesh();
            if (nav == null)
                return;

            self.crowd.Update(dt, self._agentDebug);

            // 先清理待删除的失效agent
            if (self.toRemoveAgents.Count > 0)
            {
                foreach (int agentIdx in self.toRemoveAgents)
                {
                    self.agent2unit.Remove(agentIdx);
                }
                self.toRemoveAgents.Clear();
            }

            // 同步Unit位置（仅遍历有效的映射）
            foreach (var kvp in self.agent2unit)
            {
                Unit unit = kvp.Value;
                if (unit == null)
                {
                    // 标记为待删除（不在遍历中直接修改字典）
                    self.toRemoveAgents.Add(kvp.Key);
                    continue;
                }

                DtCrowdAgent agent = self.crowd.GetAgent(kvp.Key);
                if (agent != null && agent.state != DtCrowdAgentState.DT_CROWDAGENT_STATE_INVALID)
                {
                    unit.Position = RecastToUnity(agent.npos);
                }
            }

            self._agentDebug.vod.NormalizeSamples();
        }

        public static void RemoveAgent(this CrowdComponent self, int agentId)
        {
            if (self.crowd == null)
                return;

            DtCrowdAgent agent = self.crowd.GetAgent(agentId);

            if (agent == null)
            {
                return;
            }

            self.agent2unit.Remove(agent.idx);

            self.crowd.RemoveAgent(agent);
            if (agent == self._agentDebug.agent)
            {
                self._agentDebug.agent = null;
            }
        }

        public static void AddAgent(this CrowdComponent self, Unit unit)
        {
            if (self.crowd == null)
            {
                Log.Error("AddAgent failed, crowd is null");
                return;
            }

            if (unit == null)
            {
                Log.Error("AddAgent failed, unit is null");
                return;
            }

            // 检查是否已存在
            if (unit.DtCrowdAgentId >= 0 && self.agent2unit.ContainsKey(unit.DtCrowdAgentId))
            {
                Log.Warning($"AddAgent: Unit {unit.Id} already has agent {unit.DtCrowdAgentId}, removing old one first");
                self.RemoveAgent(unit.DtCrowdAgentId);
            }

            RcVec3f p = UnityToRecast(unit.Position);

            DtCrowdAgentParams ap = new DtCrowdAgentParams();
            ap.radius = agentRadius;
            ap.height = agentHeight;
            ap.maxAcceleration = agentMaxAcceleration;
            // ap.maxSpeed = agentMaxSpeed;
            ap.collisionQueryRange = ap.radius * 12.0f;
            ap.pathOptimizationRange = ap.radius * 30.0f;
            ap.updateFlags = self._agCfg.GetUpdateFlags();
            ap.obstacleAvoidanceType = self._agCfg.obstacleAvoidanceType;
            ap.separationWeight = self._agCfg.separationWeight;

            DtCrowdAgent ag = self.crowd.AddAgent(p, ap);
            if (ag != null)
            {
                self.agent2unit.TryAdd(ag.idx, unit);
                unit.DtCrowdAgentId = ag.idx;
            }
            else
            {
                Log.Error($"AddAgent failed for unit {unit.Id} at position {unit.Position}");
            }
        }

        public static void SetMoveTarget(this CrowdComponent self, int agentId, float3 target, float speed, bool adjust = false)
        {
            if (self.crowd == null)
                return;

            DtCrowdAgent agent = self.crowd.GetAgent(agentId);

            if (agent == null)
            {
                Log.Warning($"SetMoveTarget failed, agent not found: {agentId}");
                return;
            }

            agent.option.maxSpeed = speed;

            RcVec3f p = UnityToRecast(target);
            // Find nearest point on navmesh and set move request to that location.
            DtNavMeshQuery navquery = self.crowd.GetNavMeshQuery();
            IDtQueryFilter filter = self.crowd.GetFilter(0);
            RcVec3f halfExtents = self.crowd.GetQueryExtents();

            if (adjust)
            {
                // Request velocity
                RcVec3f vel = CalcVel(agent.npos, p, agent.option.maxSpeed);
                self.crowd.RequestMoveVelocity(agent, vel);
            }
            else
            {
                long _moveTargetRef;
                RcVec3f _moveTargetPos;
                navquery.FindNearestPoly(p, halfExtents, filter, out _moveTargetRef, out _moveTargetPos, out var _);
                self.crowd.RequestMoveTarget(agent, _moveTargetRef, _moveTargetPos);
            }
        }

        public static void Stop(this CrowdComponent self, int agentId)
        {
            if (self.crowd == null)
                return;

            DtCrowdAgent agent = self.crowd.GetAgent(agentId);

            if (agent == null)
            {
                Log.Warning($"Stop agent failed, agent not found: {agentId}");
                return;
            }

            // 重置速度和目标
            self.crowd.ResetMoveTarget(agent);

            // 设置速度为0
            RcVec3f zeroVel = RcVec3f.Zero;
            self.crowd.RequestMoveVelocity(agent, zeroVel);
        }

        public static void ChangePosition(this CrowdComponent self, int agentId, float3 target)
        {
            if (self.crowd == null)
                return;

            DtCrowdAgent agent = self.crowd.GetAgent(agentId);

            if (agent == null)
            {
                Log.Warning($"ChangePosition failed, agent not found: {agentId}");
                return;
            }
            
            RcVec3f targetPos = UnityToRecast(target);

            // 找到navmesh上最近的有效点
            DtNavMeshQuery navquery = self.crowd.GetNavMeshQuery();
            IDtQueryFilter filter = self.crowd.GetFilter(0);
            RcVec3f halfExtents = self.crowd.GetQueryExtents();

            navquery.FindNearestPoly(targetPos, halfExtents, filter, out long nearestRef, out RcVec3f nearestPt, out var _);

            if (nearestRef != 0)
            {
                // 将agent传送到新位置
                agent.npos = nearestPt;
                agent.corridor.Reset(nearestRef, nearestPt);
                agent.boundary.Reset();
                agent.partial = false;

                // 重置速度
                agent.vel = RcVec3f.Zero;
                agent.dvel = RcVec3f.Zero;
            }
            else
            {
                Log.Error($"ChangePosition failed, cannot find valid position on navmesh for agent: {agentId}, target: {target}");
            }
        }

        public static void ChangeSpeed(this CrowdComponent self, int agentId, float speed)
        {
            if (self.crowd == null)
                return;

            DtCrowdAgent agent = self.crowd.GetAgent(agentId);

            if (agent == null)
            {
                Log.Warning($"ChangeSpeed failed, agent not found: {agentId}");
                return;
            }

            // 速度验证
            if (speed < 0)
            {
                Log.Warning($"ChangeSpeed: Invalid speed {speed} for agent {agentId}, clamping to 0");
                speed = 0;
            }
            else if (speed > 100f) // 合理的最大速度上限
            {
                Log.Warning($"ChangeSpeed: Speed {speed} too high for agent {agentId}, clamping to 100");
                speed = 100f;
            }

            agent.option.maxSpeed = speed;
        }

        private static RcVec3f CalcVel(RcVec3f pos, RcVec3f tgt, float speed)
        {
            RcVec3f vel = RcVec3f.Subtract(tgt, pos);
            vel.Y = 0.0f;
            vel = RcVec3f.Normalize(vel);
            return vel * speed;
        }

        // Unity坐标系 (x,y,z) -> DotRecast坐标系 (x,y,z)
        private static RcVec3f UnityToRecast(float3 unityPos)
        {
            // return new RcVec3f(-unityPos.x, unityPos.y, unityPos.z);
            return new RcVec3f(-unityPos.x, 0, unityPos.y);
        }

        // DotRecast坐标系 (x,y,z) -> Unity坐标系 (x,y,z)
        private static float3 RecastToUnity(RcVec3f recastPos)
        {
            // return new float3(-recastPos.X, recastPos.Y, recastPos.Z);
            return new float3(-recastPos.X, recastPos.Z, 0);
        }
    }
}