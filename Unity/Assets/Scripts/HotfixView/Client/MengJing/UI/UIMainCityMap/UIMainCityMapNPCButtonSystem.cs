using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainCityMapNPCButton))]
    [FriendOf(typeof(UIMainCityMapNPCButton))]
    public static partial class UIMainCityMapNPCButtonSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainCityMapNPCButton self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Button_GoToNPC = rc.Get<GameObject>("Button_GoToNPC").GetComponent<Button>();
            self.Text_NPCName = rc.Get<GameObject>("Text_NPCName").GetComponent<TMP_Text>();
        }
    }
}