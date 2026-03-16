
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
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

        }

        public static void OnRecall(this UIBattleFailureComponent self)
        {
        }
    }
}