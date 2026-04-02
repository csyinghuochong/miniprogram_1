using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class TransformSyncComponent : Entity, IAwake, IUpdate, IDestroy
    {
        private EntityRef<Unit> myUnit;
        public Unit MyUnit { get => this.myUnit; set => this.myUnit = value; }

        private EntityRef<GameObjectComponent> gameObjectComponent;
        public GameObjectComponent GameObjectComponent { get => this.gameObjectComponent; set => this.gameObjectComponent = value; }

        /// <summary>
        /// 位置平滑时间。
        /// 数值越小，位置越快追上逻辑目标点。
        /// </summary>
        public float PositionSmoothTime { get; set; } = 0.08f;
        
        /// <summary>
        /// 触发瞬移吸附的距离阈值。
        /// <remarks>当渲染位置与逻辑目标点距离超过该值时，将直接吸附到目标位置。</remarks>
        /// </summary>
        public float SnapDistance { get; set; } = 6f;

        public Vector3 TargetPosition;
        public Vector3 CurrentPosition;
        public Quaternion CurrentRotation = Quaternion.identity;
        public Vector3 PositionVelocity;
        public bool NeedSnap;
    }
}