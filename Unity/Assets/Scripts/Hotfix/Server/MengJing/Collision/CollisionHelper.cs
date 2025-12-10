using System;
using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;

namespace ET.Server
{
    public static class CollisionHelper
    {
        // 依次类推，最多 16 个类别
        public const ushort Default = 1 << 0;
        public const ushort Player = 1 << 1;
        public const ushort Monster = 1 << 2;
        public const ushort Map = 1 << 3;

        public const ushort Max = 1 << 15;
        public const ushort All = 0xFFFF;

        public static ushort GetMaskBits(ushort layer)
        {
            return layer switch
            {
                Default => All,
                Player => Player | Monster,
                _ => All
            };
        }

        /// <summary>
        /// 创建静态边界（不需要ColliderComponent）
        /// </summary>
        /// <param name="scene">场景</param>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="layer">碰撞层</param>
        /// <returns>创建的Body（可用于后续销毁）</returns>
        public static Body CreateStaticEdge(Scene scene, Vector2 start, Vector2 end, ushort layer = Default)
        {
            CollisionWorldComponent collisionWorld = scene.GetComponent<CollisionWorldComponent>();
            if (collisionWorld == null)
            {
                Log.Error("CollisionWorldComponent not found in scene!");
                return null;
            }

            // 创建静态刚体
            Body body = collisionWorld.CreateStaticBody(Vector2.Zero);

            // 创建边形状
            EdgeShape edgeShape = new();
            edgeShape.SetTwoSided(start, end);

            // 创建Fixture
            FixtureDef fixtureDef = new();
            fixtureDef.Shape = edgeShape;
            fixtureDef.Friction = 0.3f;
            fixtureDef.Restitution = 0f;
            fixtureDef.Filter = new()
            {
                CategoryBits = layer,
                MaskBits = GetMaskBits(layer),
                GroupIndex = 0
            };

            body.CreateFixture(fixtureDef);

            return body;
        }

        /// <summary>
        /// 创建静态链条边界（多条连接的边）
        /// </summary>
        /// <param name="scene">场景</param>
        /// <param name="points">顶点列表</param>
        /// <param name="isLoop">是否闭合</param>
        /// <param name="layer">碰撞层</param>
        /// <returns>创建的Body（可用于后续销毁）</returns>
        public static Body CreateStaticChain(Scene scene, List<Vector2> points, bool isLoop = false, ushort layer = Default)
        {
            CollisionWorldComponent collisionWorld = scene.GetComponent<CollisionWorldComponent>();
            if (collisionWorld == null)
            {
                Log.Error("CollisionWorldComponent not found in scene!");
                return null;
            }

            // 创建静态刚体
            Body body = collisionWorld.CreateStaticBody(Vector2.Zero);

            // 创建链条形状
            ChainShape chainShape = new();
            if (isLoop)
            {
                chainShape.CreateLoop(points.ToArray());
            }
            else
            {
                // 非闭合链条需要指定ghost vertices（首尾连接点）
                // 使用链条两端延伸的虚拟点
                Vector2 prevVertex = points[0] - (points[1] - points[0]);
                Vector2 nextVertex = points[^1] + (points[^1] - points[^2]);
                chainShape.CreateChain(points.ToArray(), points.Count, prevVertex, nextVertex);
            }

            // 创建Fixture
            FixtureDef fixtureDef = new();
            fixtureDef.Shape = chainShape;
            fixtureDef.Friction = 0.3f;
            fixtureDef.Restitution = 0f;
            fixtureDef.Filter = new()
            {
                CategoryBits = layer,
                MaskBits = GetMaskBits(layer),
                GroupIndex = 0
            };

            body.CreateFixture(fixtureDef);

            return body;
        }
    }
}