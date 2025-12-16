using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [EnableClass]
    public class RingBuffer<T>
    {
        private readonly T[] buffer;
        private int head;
        private int count;

        public int Count => count;
        public int Capacity => buffer.Length;

        public RingBuffer(int capacity)
        {
            buffer = new T[capacity];
            head = 0;
            count = 0;
        }

        public void Add(T item)
        {
            buffer[head] = item;
            head = (head + 1) % buffer.Length;
            if (count < buffer.Length)
                count++;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();

                int idx = (head - 1 - index + buffer.Length) % buffer.Length;
                return buffer[idx];
            }
        }

        public void Clear()
        {
            head = 0;
            count = 0;
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

        // 环形缓冲区替代 List
        public RingBuffer<PositionState> PositionBuffer = new(50);
        public RingBuffer<RotationState> RotationBuffer = new(50);
        public float InterpolationBackTime = 0.1f; // 延迟100ms进
    }
}