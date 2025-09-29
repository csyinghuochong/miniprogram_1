using System;
using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.UIItemTip)]
    public class UIItemTipEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIItemTip);
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIItemTip, gameObject);
            ui.AddComponent<UIItemTipComponent>();
            return ui;
        }

        public override void OnRemove(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIItemTip);
            scene.GetComponent<ResourcesLoaderComponent>().UnLoadAsset(path);
        }
    }
}