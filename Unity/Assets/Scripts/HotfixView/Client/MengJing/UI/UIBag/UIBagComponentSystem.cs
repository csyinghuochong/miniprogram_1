using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIBagComponent))]
    [FriendOf(typeof(UIBagComponent))]
    public static partial class UIBagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIBagComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIBag); });
        }
    }
}