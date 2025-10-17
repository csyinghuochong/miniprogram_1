using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class EnterMapFinish_CreateUIMain : AEvent<Scene, EnterMapFinish>
    {
        protected override async ETTask Run(Scene root, EnterMapFinish args)
        {
            await root.GetComponent<UIComponent>().Create(UIType.UIMain);
            root.GetComponent<UIComponent>().Remove(UIType.UICreateRole);
            root.GetComponent<UIComponent>().Remove(UIType.UILogin);

            // HeroHelper.Test_CreateMyHeroes(root);
            // HeroHelper.Test_CreateMonsters(root);
        }
    }
}