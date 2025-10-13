using TMPro;
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
        
        public TMP_Text Text_FPS;
        public Button Button_Speed;
        public Button Button_GM;
        public Button Button_Team;
        public Button Button_Bag;
    }
}