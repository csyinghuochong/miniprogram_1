using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    public struct FixedStateBuffer<T> where T : struct
    {
        private const int MAX_CAPACITY = 30;
        private T[] buffer; // 只分配一次，池化复用
        private int writeIndex;
        private int count;

        public int Count => count;

        public void EnsureInitialized()
        {
            if (buffer == null)
            {
                buffer = new T[MAX_CAPACITY];
                writeIndex = 0;
                count = 0;
            }
        }

        public void Add(T item)
        {
            EnsureInitialized();
            buffer[writeIndex] = item;
            writeIndex = (writeIndex + 1) % MAX_CAPACITY;
            if (count < MAX_CAPACITY)
                count++;
        }

        public ref T GetRef(int index)
        {
            int idx = (writeIndex - 1 - index + MAX_CAPACITY) % MAX_CAPACITY;
            return ref buffer[idx];
        }

        public void Clear()
        {
            writeIndex = 0;
            count = 0;
            // 注意：不释放buffer数组，保留复用
        }
    }

    public struct PositionState
    {
        public float Timestamp;
        public Vector3 Position;
    }

    public struct RotationState
    {
        public float Timestamp;
        public Quaternion Rotation;
    }

    [ComponentOf(typeof(Unit))]
    public class TransformSyncComponent : Entity, IAwake, IUpdate, IDestroy
    {
        private EntityRef<Unit> myUnit;
        public Unit MyUnit { get => this.myUnit; set => this.myUnit = value; }

        private EntityRef<GameObjectComponent> gameObjectComponent;
        public GameObjectComponent GameObjectComponent { get => this.gameObjectComponent; set => this.gameObjectComponent = value; }

        private EntityRef<FsmComponent> fsmComponent;
        public FsmComponent FsmComponent { get => this.fsmComponent; set => this.fsmComponent = value; }

        public FixedStateBuffer<PositionState> PositionBuffer;
        public FixedStateBuffer<RotationState> RotationBuffer;

        public FsmStateEnum cachedFsmState = FsmStateEnum.FsmIdleState;
    }
}