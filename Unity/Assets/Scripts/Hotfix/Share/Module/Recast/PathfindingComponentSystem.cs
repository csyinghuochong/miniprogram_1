using System;
using System.Collections.Generic;
using System.IO;
using DotRecast.Core;
using DotRecast.Core.Numerics;
using DotRecast.Detour;
using DotRecast.Detour.Io;
using Unity.Mathematics;

namespace ET
{
    [EntitySystemOf(typeof(PathfindingComponent))]
    [FriendOf(typeof(PathfindingComponent))]
    public static partial class PathfindingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PathfindingComponent self, byte[] buffer)
        {
            DtMeshSetReader reader = new();
            using MemoryStream ms = new(buffer);
            using BinaryReader br = new(ms);
            self.navMesh = reader.Read32Bit(br, 6); // cpp recast导出来的要用Read32Bit读取，DotRecast导出来的还没试过

            if (self.navMesh == null)
            {
                throw new Exception($"nav load fail");
            }

            self.filter = new DtQueryDefaultFilter();
            self.query = new DtNavMeshQuery(self.navMesh);
        }

        [EntitySystem]
        private static void Destroy(this PathfindingComponent self)
        {
            self.navMesh = null;
        }

        public static void Find(this PathfindingComponent self, float3 start, float3 target, List<float3> result)
        {
            if (self.navMesh == null)
            {
                Log.Debug("寻路| Find 失败 pathfinding ptr is zero");
                throw new Exception($"pathfinding ptr is zero: {self.Scene().Name}");
            }

            RcVec3f startPos = UnityToRecast(start);
            RcVec3f endPos = UnityToRecast(target);

            long startRef;
            long endRef;
            RcVec3f startPt;
            RcVec3f endPt;

            self.query.FindNearestPoly(startPos, self.extents, self.filter, out startRef, out startPt, out _);
            self.query.FindNearestPoly(endPos, self.extents, self.filter, out endRef, out endPt, out _);

            Span<long> polysSpan = stackalloc long[PathfindingComponent.MAX_POLYS];
            DtStatus status = self.query.FindPath(startRef, endRef, startPt, endPt, self.filter, polysSpan, out int polyCount, PathfindingComponent.MAX_POLYS);

            if (!status.Succeeded() || polyCount <= 0)
            {
                return;
            }

            // In case of partial path, make sure the end point is clamped to the last polygon.
            RcVec3f epos = new RcVec3f(endPt.X, endPt.Y, endPt.Z);
            if (polysSpan[polyCount - 1] != endRef)
            {
                DtStatus dtStatus = self.query.ClosestPointOnPoly(polysSpan[polyCount - 1], endPt, out RcVec3f closest, out bool _);
                if (dtStatus.Succeeded())
                {
                    epos = closest;
                }
            }

            Span<DtStraightPath> straightPath = stackalloc DtStraightPath[PathfindingComponent.MAX_POLYS];
            self.query.FindStraightPath(startPt, epos, polysSpan, polyCount, straightPath, out int straightPathCount, PathfindingComponent.MAX_POLYS, DtStraightPathOptions.DT_STRAIGHTPATH_ALL_CROSSINGS);

            // 预分配容量避免List扩容产生GC
            if (result.Capacity < straightPathCount)
            {
                result.Capacity = straightPathCount;
            }

            for (int i = 0; i < straightPathCount; ++i)
            {
                RcVec3f pos = straightPath[i].pos;
                result.Add(RecastToUnity(pos));
            }
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