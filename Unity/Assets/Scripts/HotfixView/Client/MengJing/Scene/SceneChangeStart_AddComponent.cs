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
                if (args.SceneType == MapTypeEnum.LocalLevel)
                {
                    GlobalComponent.Instance.ViewMode = 1;
                }
                else
                {
                    GlobalComponent.Instance.ViewMode = 0;
                }
                
                root.GetComponent<SceneManagerComponent>().BeforeChangeScene();

                UI ui = await root.GetComponent<UIComponent>().Create(UIType.UILoading);
                ui.GetComponent<UILoadingComponent>().OnInitUI();

                ui = root.GetComponent<UIComponent>().Get(UIType.UIMain);
                if (ui != null)
                {
                    ui.GetComponent<UIMainComponent>().BeforeEnterScene(args.LastSceneType);
                }

                Log.Debug($"SceneChangeStart:  {args.LastSceneType}");

                await root.GetComponent<SceneManagerComponent>().ChangeScene(args.SceneType, args.LastSceneType, args.ChapterId);

                // root.AddComponent<OperaComponent>();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}