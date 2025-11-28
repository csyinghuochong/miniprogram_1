using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIDialogueComponent))]
    [FriendOf(typeof(UIDialogueComponent))]
    public static partial class UIDialogueComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIDialogueComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_NpcName = rc.Get<GameObject>("Text_NpcName").GetComponent<TMP_Text>();
            self.Text_Content = rc.Get<GameObject>("Text_Content").GetComponent<TMP_Text>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIDialogue); });
        }

        public static async ETTask UpdateDialogue(this UIDialogueComponent self, int npcId)
        {
            NPCConfig npcConfig = NPCConfigCategory.Instance.Get(npcId);

            self.Text_NpcName.SetText(npcConfig.Name);
            self.Text_Content.SetText(npcConfig.DialogueText);
        }
    }
}