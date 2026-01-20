using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainCityMapComponent))]
    [FriendOf(typeof(UIMainCityMapComponent))]
    public static partial class UIMainCityMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainCityMapComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIMainCityMapNPCButton = rc.Get<GameObject>("Content_UIMainCityMapNPCButton").transform;
            self.UIMainCityMapNPCButton = rc.Get<GameObject>("UIMainCityMapNPCButton");
            self.UIMainCityMapNPCButton.SetActive(false);

            self.Button_Close.AddListener((() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIMainCityMap); }));
        }

        [EntitySystem]
        private static void Destroy(this UIMainCityMapComponent self)
        {
            self.UIMainCityMapNPCButtonList.Clear();
            self.UIMainCityMapNPCButton = null;
        }

    }
}