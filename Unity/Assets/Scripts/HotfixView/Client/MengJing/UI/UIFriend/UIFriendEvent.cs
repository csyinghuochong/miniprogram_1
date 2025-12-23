using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.UIFriend)]
    public class UIFriendEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIFriend);
            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIFriend, gameObject);
            ui.AddComponent<UIFriendComponent>();
            return ui;
        }

        public override void OnRemove(Scene scene, UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIFriend);
            scene.GetComponent<ResourcesLoaderComponent>().UnLoadAsset(path);
        }
    }
}