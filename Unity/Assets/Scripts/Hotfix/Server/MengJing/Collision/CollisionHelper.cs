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
    }
}