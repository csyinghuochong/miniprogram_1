using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class BattleFailure_UIMainRefresh : AEvent<Scene, BattleFailure>
    {
        protected override async ETTask Run(Scene scene, BattleFailure args)
        {
            UI ui = await scene.GetComponent<UIComponent>().Create(UIType.UIBattleFailure);
            UIBattleFailureComponent uiBattleFailureComponent = ui.GetComponent<UIBattleFailureComponent>();

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIBattleFailureComponent))]
    [FriendOf(typeof(UIBattleFailureComponent))]
    public static partial class UIBattleFailureComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIBattleFailureComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Rechallenge = rc.Get<GameObject>("Button_Rechallenge").GetComponent<Button>();
            self.Button_Recall = rc.Get<GameObject>("Button_Recall").GetComponent<Button>();

            self.Button_Rechallenge.AddListener(() => { self.OnRechallenge(); });
            self.Button_Recall.AddListener(() => { self.OnRecall(); });
        }

        public static void OnRechallenge(this UIBattleFailureComponent self)
        {
            ClientLevelHelper.RequestRechallenge(self.Root()).Coroutine();
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIBattleFailure);
        }

        public static void OnRecall(this UIBattleFailureComponent self)
        {
            EnterMapHelper.RequestTransfer(self.Root(), MapType.MainCity).Coroutine();
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIBattleFailure);
        }
    }
}