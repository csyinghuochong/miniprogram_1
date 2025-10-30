using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILoadingComponent))]
    [FriendOf(typeof(UILoadingComponent))]
    public static partial class UILoadingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILoadingComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Slider_Progress = rc.Get<GameObject>("Slider_Progress").GetComponent<Slider>();
            self.Text_Progress = rc.Get<GameObject>("Text_Progress").GetComponent<TMP_Text>();
        }

        [EntitySystem]
        private static void Destroy(this UILoadingComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this UILoadingComponent self)
        {
            if (self.Program < 0.3)
            {
                self.Program += Time.deltaTime * 0.1f;
            }

            if (!ConfigData.LoadSceneFinished)
            {
                self.ShowProgress(self.Program);
                return;
            }

            if (!self.StartLoadAssets)
            {
                self.StartLoadAssets = true;
                self.StartPreLoadAssets().Coroutine();
                UnitFactory.ShowAllUnit(self.Root()).Coroutine();
            }

            if (self.PreLoadAssets.Count > 0)
            {
                self.ShowProgress(0.8f);
                return;
            }
            else
            {
                self.ShowProgress(1f);
            }

            self.PassTime += Time.deltaTime;
            self.ShowProgress(0.9f + self.PassTime * 0.1f);

            if (self.PassTime < 1.5f)
            {
                return;
            }

            Unit main = UnitHelper.GetMyUnitFromClientScene(self.Root());
            if (main == null)
            {
                return;
            }

            UI ui = self.Root().GetComponent<UIComponent>().Get(UIType.UIMain);
            if (ui != null)
            {
                ui.GetComponent<UIMainComponent>().AfterEnterScene(self.Root().GetComponent<MapComponent>().MapType);
            }

            // Camera camera = self.Root().GetComponent<GlobalComponent>().MainCamera.GetComponent<Camera>();
            // camera.GetComponent<Camera>().fieldOfView = 50;

            self.Root().GetComponent<UIComponent>().Remove(UIType.UILoading);
        }

        public static void OnInitUI(this UILoadingComponent self)
        {
            // 可以设置一些要提前加载的资源
        }

        private static async ETTask StartPreLoadAssets(this UILoadingComponent self)
        {
            for (int i = self.PreLoadAssets.Count - 1; i >= 0; i--)
            {
                await self.Root().GetComponent<GameObjectLoadComponent>().PreLoadQueue(self.PreLoadAssets[i]);
                self.PreLoadAssets.RemoveAt(i);
            }
        }

        private static void ShowProgress(this UILoadingComponent self, float progress)
        {
            if (progress < 0)
            {
                progress = 0;
            }

            if (progress > 1)
            {
                progress = 1;
            }

            self.Slider_Progress.value = progress;
            self.Text_Progress.SetTextFormat("{0}%", (int)(progress * 100));
        }
    }
}