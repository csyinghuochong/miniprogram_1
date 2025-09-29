using System;
using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.UIBag)]
    public class UIBagEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIBag);
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIBag, gameObject);
            ui.AddComponent<UIBagComponent>();
            return ui;
        }

        public override void OnRemove(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIBag);
            scene.GetComponent<ResourcesLoaderComponent>().UnLoadAsset(path);
        }
    }
}