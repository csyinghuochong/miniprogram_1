using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainComponent))]
    [FriendOf(typeof(UIMainComponent))]
    public static partial class UIMainComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_FPS = rc.Get<GameObject>("Text_FPS").GetComponent<TMP_Text>();
            self.Button_Speed = rc.Get<GameObject>("Button_Speed").GetComponent<Button>();
            self.Button_GM = rc.Get<GameObject>("Button_GM").GetComponent<Button>();
            self.Button_Team = rc.Get<GameObject>("Button_Team").GetComponent<Button>();
            self.Button_Bag = rc.Get<GameObject>("Button_Bag").GetComponent<Button>();

            self.Button_Speed.onClick.AddListener(() => { self.OnButton_Speed(); });
            self.Button_GM.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIGM).Coroutine(); });
            self.Button_Team.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UITeam).Coroutine(); });
            self.Button_Bag.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIBag).Coroutine(); });

            Application.targetFrameRate = 60;
        }

        [EntitySystem]
        private static void Update(this UIMainComponent self)
        {
            self.TimeLeft -= Time.deltaTime;
            self.Accumulator += Time.timeScale / Time.deltaTime;
            self.FrameCount++;
            if (self.TimeLeft <= 0f)
            {
                self.FPS = self.Accumulator / self.FrameCount;
                self.Text_FPS.SetText("FPS:{0}", (int)self.FPS);

                self.TimeLeft = self.UpdateInterval;
                self.Accumulator = 0f;
                self.FrameCount = 0;
            }
        }

        private static void OnButton_Speed(this UIMainComponent self)
        {
            self.SpeedLevel++;
            if (self.SpeedLevel > 3)
            {
                self.SpeedLevel = 0;
            }

            switch (self.SpeedLevel)
            {
                case 0:
                    Time.timeScale = 0f;
                    break;
                case 1:
                    Time.timeScale = 1f;
                    break;
                case 2:
                    Time.timeScale = 2f;
                    break;
                case 3:
                    Time.timeScale = 3f;
                    break;
            }

            self.Button_Speed.GetComponentInChildren<TMP_Text>().SetText("x{0}", self.SpeedLevel);
        }
    }
}