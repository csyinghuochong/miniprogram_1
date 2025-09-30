using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIGMComponent))]
    [FriendOf(typeof(UIGMComponent))]
    public static partial class UIGMComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIGMComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.InputField_AddItem_ItemId = rc.Get<GameObject>("InputField_AddItem_ItemId").GetComponent<TMP_InputField>();
            self.InputField_AddItem_ItemNum = rc.Get<GameObject>("InputField_AddItem_ItemNum").GetComponent<TMP_InputField>();
            self.Button_AddItem_Send = rc.Get<GameObject>("Button_AddItem_Send").GetComponent<Button>();
            self.InputField_AddHero_HeroId = rc.Get<GameObject>("InputField_AddHero_HeroId").GetComponent<TMP_InputField>();
            self.Button_AddHero_Send = rc.Get<GameObject>("Button_AddHero_Send").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIGM); });
            self.Button_AddItem_Send.onClick.AddListener(() => { self.OnAddItem_Send(); });
            self.Button_AddHero_Send.onClick.AddListener(() => { self.OnAddHero_Send(); });
        }

        private static void OnAddItem_Send(this UIGMComponent self)
        {
            string msg = "1#" + self.InputField_AddItem_ItemId.text + "#" + self.InputField_AddItem_ItemNum.text;

            GMHelp.SendGmCommand(self.Root(), msg);
        }

        private static void OnAddHero_Send(this UIGMComponent self)
        {
            string msg = "2#" + self.InputField_AddHero_HeroId.text;

            GMHelp.SendGmCommand(self.Root(), msg);
        }
    }
}