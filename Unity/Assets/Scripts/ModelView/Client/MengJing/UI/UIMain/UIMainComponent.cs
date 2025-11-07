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

        public UIJoystickComponent UIJoystickComponent { get; set; }

        public GameObject UIJoystick;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_PlayerLv;
        public TMP_Text Text_FPS;
        public TMP_Text Text_Gold;
        public TMP_Text Text_Diamond;
        public Image Image_TaskCompleted;
        public TMP_Text Text_TaskName;
        public TMP_Text Text_TaskProgress;
        public Button Button_TaskCommit;
        public Button Button_TaskReward;
        public Button Button_Recall;
        public Button Button_StartLevel;
        public Button Button_Speed;
        public Button Button_GM;
        public Button Button_Hero;
        public Button Button_Bag;
        public GameObject UIMainSkill;
        public Button Button_Boss;
        public Slider Slider_Exp;
        public TMP_Text Text_Exp;
    }
}