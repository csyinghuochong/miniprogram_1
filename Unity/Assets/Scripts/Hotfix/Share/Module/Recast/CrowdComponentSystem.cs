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

        const float maxAgentRadius = 2f;
        const float agentHeight = 2f;
        private const float agentMaxAcceleration = 8f;
        const int SAMPLE_POLYFLAGS_DISABLED = 0x10; // Disabled polygon
        const int SAMPLE_POLYFLAGS_ALL = 0xffff; // All abilities.

        [EntitySystem]
        private static void Awake(this CrowdComponent self, byte[] buffer)
        {
            DtMeshSetReader reader = new();
            using MemoryStream ms = new(buffer);
            using BinaryReader br = new(ms);
            self.NavMesh = reader.Read32Bit(br, 6); // cpp recast导出来的要用Read32Bit读取，DotRecast导出来的还没试过

            if (self.NavMesh == null)
            {
                throw new Exception($"nav load fail");
            }

            self.AgCfg = new DtCrowdAgentConfig();
            self.AgentDebug = new DtCrowdAgentDebugInfo();
            self.AgentDebug.vod = new DtObstacleAvoidanceDebugData(2048);

            self.Setup(self.NavMesh);

            self.LastUpdateTime = TimeInfo.Instance.ClientNow();
            self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.CrowdTimer, self);
        }

        [EntitySystem]
        private static void Destroy(this CrowdComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.Agent2Unit.Clear();
            self.ToRemoveAgents.Clear();
            self.NavMesh = null;
            self.Crowd = null;
        }

        private static void Setup(this CrowdComponent self, DtNavMesh navMesh)
        {
            DtCrowdConfig config = new DtCrowdConfig(maxAgentRadius);
            self.Crowd = new DtCrowd(config, navMesh, __ => new DtQueryDefaultFilter(SAMPLE_POLYFLAGS_ALL,
                SAMPLE_POLYFLAGS_DISABLED,
                new float[] { 1f, 10f, 1f, 1f, 2f, 1.5f }));

            // Setup local avoidance option to different qualities.
            // Use mostly default settings, copy from dtCrowd.
            DtObstacleAvoidanceParams option = new DtObstacleAvoidanceParams(self.Crowd.GetObstacleAvoidanceParams(0));

            // Low (11)
            option.velBias = 0.5f;
            option.adaptiveDivs = 5;
            option.adaptiveRings = 2;
            option.adaptiveDepth = 1;
            self.Crowd.SetObstacleAvoidanceParams(0, option);

            // Medium (22)
            option.velBias = 0.5f;
            option.adaptiveDivs = 5;
            option.adaptiveRings = 2;
            option.adaptiveDepth = 2;
            self.Crowd.SetObstacleAvoidanceParams(1, option);

            // Good (45)
            option.velBias = 0.5f;
            option.adaptiveDivs = 7;
            option.adaptiveRings = 2;
            option.adaptiveDepth = 3;
            self.Crowd.SetObstacleAvoidanceParams(2, option);

            // High (66)
            option.velBias = 0.5f;
            option.adaptiveDivs = 7;
            option.adaptiveRings = 3;
            option.adaptiveDepth = 3;

            self.Crowd.SetObstacleAvoidanceParams(3, option);
        }

        private static void Update(this CrowdComponent self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float dt = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            if (self.Crowd == null)
            {
                return;
            }

            DtNavMesh nav = self.Crowd.GetNavMesh();
            if (nav == null)
            {
                return;
            }

            self.Crowd.Update(dt, self.AgentDebug);

            // 先清理待删除的失效agent
            if (self.ToRemoveAgents.Count > 0)
            {
                foreach (int agentIdx in self.ToRemoveAgents)
                {
                    self.Agent2Unit.Remove(agentIdx);
                }

                self.ToRemoveAgents.Clear();
            }

            // 同步Unit位置（仅遍历有效的映射）
            foreach (var kvp in self.Agent2Unit)
            {
                Unit unit = kvp.Value;
                if (unit == null)
                {
                    // 标记为待删除（不在遍历中直接修改字典）
                    self.ToRemoveAgents.Add(kvp.Key);
                    continue;
                }

                DtCrowdAgent agent = self.Crowd.GetAgent(kvp.Key);
                if (agent != null && agent.state != DtCrowdAgentState.DT_CROWDAGENT_STATE_INVALID)
                {
                    unit.Position = RecastToUnity(agent.npos);
                }
            }

            self.AgentDebug.vod.NormalizeSamples();
        }

        public static void RemoveAgent(this CrowdComponent self, int agentId)
        {
            if (self.Crowd == null)
            {
                return;
            }

            DtCrowdAgent agent = self.Crowd.GetAgent(agentId);

            if (agent == null)
            {
                return;
            }

            self.Agent2Unit.Remove(agent.idx);

            self.Crowd.RemoveAgent(agent);
            if (agent == self.AgentDebug.agent)
            {
                self.AgentDebug.agent = null;
            }
        }

        public static void AddAgent(this CrowdComponent self, Unit unit)
        {
            if (self.Crowd == null)
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
            if (unit.DtCrowdAgentId >= 0 && self.Agent2Unit.ContainsKey(unit.DtCrowdAgentId))
            {
                Log.Warning($"AddAgent: Unit {unit.Id} already has agent {unit.DtCrowdAgentId}, removing old one first");
                self.RemoveAgent(unit.DtCrowdAgentId);
            }

            RcVec3f p = UnityToRecast(unit.Position);

            float radius = 0.5f;
            if (unit.Type == UnitType.Hero)
            {
                if (HeroConfigCategory.Instance.DataMap.ContainsKey(unit.ConfigId))
                {
                    radius = HeroConfigCategory.Instance.Get(unit.ConfigId).Radius;
                }
            }
            else if (unit.Type == UnitType.Monster)
            {
                if (MonsterConfigCategory.Instance.DataMap.ContainsKey(unit.ConfigId))
                {
                    radius = MonsterConfigCategory.Instance.Get(unit.ConfigId).Radius;
                }
            }

            DtCrowdAgentParams ap = new DtCrowdAgentParams();
            ap.radius = radius;
            ap.height = agentHeight;
            ap.maxAcceleration = agentMaxAcceleration;
            // ap.maxSpeed = agentMaxSpeed;
            ap.collisionQueryRange = ap.radius * 12.0f;
            ap.pathOptimizationRange = ap.radius * 30.0f;
            ap.updateFlags = self.AgCfg.GetUpdateFlags();
            ap.obstacleAvoidanceType = self.AgCfg.obstacleAvoidanceType;
            ap.separationWeight = self.AgCfg.separationWeight;

            DtCrowdAgent ag = self.Crowd.AddAgent(p, ap);
            if (ag != null)
            {
                self.Agent2Unit.TryAdd(ag.idx, unit);
                unit.DtCrowdAgentId = ag.idx;
            }
            else
            {
                Log.Error($"AddAgent failed for unit {unit.Id} at position {unit.Position}");
            }
        }

        public static void SetMoveTarget(this CrowdComponent self, int agentId, float3 target, float speed, bool adjust = false)
        {
            if (self.Crowd == null)
            {
                return;
            }

            DtCrowdAgent agent = self.Crowd.GetAgent(agentId);

            if (agent == null)
            {
                return;
            }

            agent.option.maxSpeed = speed;

            RcVec3f p = UnityToRecast(target);
            // Find nearest point on navmesh and set move request to that location.
            DtNavMeshQuery navquery = self.Crowd.GetNavMeshQuery();
            IDtQueryFilter filter = self.Crowd.GetFilter(0);
            RcVec3f halfExtents = self.Crowd.GetQueryExtents();

            if (adjust)
            {
                // Request velocity
                RcVec3f vel = CalcVel(agent.npos, p, agent.option.maxSpeed);
                self.Crowd.RequestMoveVelocity(agent, vel);
            }
            else
            {
                long _moveTargetRef;
                RcVec3f _moveTargetPos;
                navquery.FindNearestPoly(p, halfExtents, filter, out _moveTargetRef, out _moveTargetPos, out var _);
                self.Crowd.RequestMoveTarget(agent, _moveTargetRef, _moveTargetPos);
            }
        }

        public static void Stop(this CrowdComponent self, int agentId)
        {
            if (self.Crowd == null)
            {
                return;
            }

            DtCrowdAgent agent = self.Crowd.GetAgent(agentId);

            if (agent == null)
            {
                return;
            }

            // 重置速度和目标
            self.Crowd.ResetMoveTarget(agent);

            // 设置速度为0
            RcVec3f zeroVel = RcVec3f.Zero;
            self.Crowd.RequestMoveVelocity(agent, zeroVel);
        }

        public static void ChangePosition(this CrowdComponent self, int agentId, float3 target)
        {
            if (self.Crowd == null)
            {
                return;
            }

            DtCrowdAgent agent = self.Crowd.GetAgent(agentId);

            if (agent == null)
            {
                return;
            }

            RcVec3f targetPos = UnityToRecast(target);

            // 找到navmesh上最近的有效点
            DtNavMeshQuery navquery = self.Crowd.GetNavMeshQuery();
            IDtQueryFilter filter = self.Crowd.GetFilter(0);
            RcVec3f halfExtents = self.Crowd.GetQueryExtents();

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
            if (self.Crowd == null)
            {
                return;
            }

            DtCrowdAgent agent = self.Crowd.GetAgent(agentId);

            if (agent == null)
            {
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