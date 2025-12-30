using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.UIRank)]
    public class UIRankEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIRank);
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIRank, gameObject);
            ui.AddComponent<UIRankComponent>();
            return ui;
        }

        public override void OnRemove(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIRank);
            scene.GetComponent<ResourcesLoaderComponent>().UnLoadAsset(path);
        }
    }
}