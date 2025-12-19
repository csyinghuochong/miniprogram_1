using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIChatComponent))]
    [FriendOf(typeof(UIChatComponent))]
    public static partial class UIChatComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIChatComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_Title = rc.Get<GameObject>("Text_Title").GetComponent<TMP_Text>();
            self.Content_UIChatItem = rc.Get<GameObject>("Content_UIChatItem").transform;
            self.UIChatItem = rc.Get<GameObject>("UIChatItem");
            self.InputField_Content = rc.Get<GameObject>("InputField_Content").GetComponent<TMP_InputField>();
            self.Button_Emoji = rc.Get<GameObject>("Button_Emoji").GetComponent<Button>();
            self.Button_Send = rc.Get<GameObject>("Button_Send").GetComponent<Button>();
            self.Button_World = rc.Get<GameObject>("Button_World").GetComponent<Button>();
            self.Button_LianMeng = rc.Get<GameObject>("Button_LianMeng").GetComponent<Button>();
        }
        
        
    }
}