using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIMainComponent : Entity, IAwake, IUpdate
    {
        public float UpdateInterval = 0.5f;
        public float Accumulator = 0f; // 帧数累加器
        public int FrameCount = 0; // 帧数计数
        public float TimeLeft; // 距离下次更新的时间
        public float FPS; // 当前帧率

        public int SpeedLevel = 1;

        public UIJoystickComponent UIJoystickComponent { get; set; }

        public GameObject UIJoystick;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_PlayerLv;
        public TMP_Text Text_FPS;
        public TMP_Text Text_Gold;
        public TMP_Text Text_Diamond;
        public Button Button_Speed;
        public Button Button_GM;
        public Button Button_Hero;
        public Button Button_Bag;
        public Slider Slider_Exp;
        public TMP_Text Text_Exp;
    }
}