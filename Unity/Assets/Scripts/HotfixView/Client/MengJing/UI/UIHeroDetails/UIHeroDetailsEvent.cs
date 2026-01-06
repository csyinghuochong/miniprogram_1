using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.UIHeroDetails)]
    public class UIHeroDetailsEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIHeroDetails);
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIHeroDetails, gameObject);
            ui.AddComponent<UIHeroDetailsComponent>();
            return ui;
        }

        public override void OnRemove(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIHeroDetails);
            scene.GetComponent<ResourcesLoaderComponent>().UnLoadAsset(path);
        }
    }
}