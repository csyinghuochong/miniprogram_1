using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public enum EJoystickModel
    {
        Fixed,
        Free
    }

    /// <summary>
    /// 摇杆
    /// </summary>
    [ComponentOf(typeof(UIMainComponent))]
    public class UIJoystickComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject;
        public GameObject StartArea;
        public GameObject Joystick;
        public GameObject JoystickBottom;
        public GameObject PositionFocus;
        public GameObject JoystickThumb;

        public Image JoystickBottomImg;
        public Image JoystickThumbImg;
        public RectTransform RectTransform;

        public Camera UICamera;
        public Camera MainCamera;
        private EntityRef<Unit> myUnit;
        public Unit MyUnit { get => this.myUnit; set => this.myUnit = value; }

        public int MapMask;
        public Vector2 OldPoint;
        public Vector2 NewPoint;
        public EJoystickModel JoystickModel;
        public float Radius; // 摇杆按钮移动的半径

        public long JoystickTimer;

        public Vector2 Direction; // 方向单位向量
        public Vector2 LastDirection;
        public Vector2 Target;
        public bool IsDrag;
        public float3 LastUnitPosition;

        public LayerMask MovableAreaMask;
        public int MaxSearchSteps;
        public float SearchStepDistance;
    }
}