using System;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeStart_AddComponent : AEvent<Scene, SceneChangeStart>
    {
        protected override async ETTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                ConfigData.ViewMode = args.MapType == MapType.LocalLevel ? 1 : 0;

                root.GetComponent<SceneManagerComponent>().BeforeChangeScene();

                UI ui = await root.GetComponent<UIComponent>().Create(UIType.UILoading);
                ui.GetComponent<UILoadingComponent>().OnInitUI();

                ui = root.GetComponent<UIComponent>().Get(UIType.UIMain);
                if (ui != null)
                {
                    ui.GetComponent<UIMainComponent>().BeforeEnterScene(args.LastMapType);
                }

                Log.Debug($"SceneChangeStart:  {args.LastMapType}");
                
                await root.GetComponent<SceneManagerComponent>().ChangeScene(args.MapType, args.LastMapType, args.ChapterId);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}