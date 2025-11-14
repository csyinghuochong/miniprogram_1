using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class OperaComponent : Entity, IAwake, IUpdate
    {
        public bool IsPaused = false;

        public Vector3 ClickPoint;

        public float LastSendTime;

        public int NpcId;
        public Vector3 UnitStartPosition;

        public Camera MainCamera;

        public bool ClickMode;
        public bool EditorMode;

        private EntityRef<Unit> unit;
        public Unit MainUnit { get => this.unit; set => this.unit = value; }
    }
}