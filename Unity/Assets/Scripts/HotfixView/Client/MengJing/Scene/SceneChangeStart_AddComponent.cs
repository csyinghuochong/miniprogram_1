using System;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeStart_AddComponent : AEvent<Scene, SceneChangeStart>
    {
        protected override async ETTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                root.GetComponent<SceneManagerComponent>().BeforeChangeScene();

                UI ui = await root.GetComponent<UIComponent>().Create(UIType.UILoading);
                ui.GetComponent<UILoadingComponent>().OnInitUI();

                Log.Debug($"SceneChangeStart:  {args.LastSceneType}");

                await root.GetComponent<SceneManagerComponent>().ChangeScene(args.SceneType, args.LastSceneType, args.ChapterId);

                root.AddComponent<OperaComponent>();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}