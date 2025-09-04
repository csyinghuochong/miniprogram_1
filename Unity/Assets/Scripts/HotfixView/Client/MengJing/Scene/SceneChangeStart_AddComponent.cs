using System;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeStart_AddComponent: AEvent<Scene, SceneChangeStart>
    {
        protected override async ETTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                root.GetComponent<SceneManagerComponent>().BeforeChangeScene();
                        
                UIComponent uiComponent = root.GetComponent<UIComponent>();
                await uiComponent.ShowWindowAsync(WindowID.WindowID_Loading);
                // uiComponent.GetDlgLogic<DlgLoading>().OnInitUI(args.LastSceneType, args.SceneType, args. ChapterId);
                //
                // DlgMain dlgMain = uiComponent.GetDlgLogic<DlgMain>();
                // if (dlgMain != null)
                // {
                //     uiComponent.CloseWindow(WindowID.WindowID_MapBig);
                //     dlgMain.BeforeEnterScene(args.LastSceneType);
                // }
                
                Log.Debug($"SceneChangeStart:  {args.LastSceneType}");

                
                await root.GetComponent<SceneManagerComponent>().ChangeScene(args.SceneType, args.LastSceneType, args.ChapterId);

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}